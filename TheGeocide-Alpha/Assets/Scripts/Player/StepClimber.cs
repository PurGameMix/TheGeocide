using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
//Also requires some sort of collider, made with an AABB in mind

/// The class that takes care of all the player related physics
/// Many configurable parameters with defaults set as the recommended values in BBR
public class StepClimber : MonoBehaviour
{
    //Remember 1 unit is 1 meter (the physics engine was made with this ratio in mind)
    [SerializeField]
    private Rigidbody2D PlayerRigidBody;

    [SerializeField]
    private LayerMask _stepLayer;

    [Header("Steps")]
    public float maxStepHeight = 0.2f;              ///< The maximum a player can set upwards in units when they hit a wall that's potentially a step
    public float stepSearchOvershoot = 0.01f;       ///< How much to overshoot into the direction a potential step in units when testing. High values prevent player from walking up small steps but may cause problems.

    private List<ContactPoint2D> allCPs = new List<ContactPoint2D>();
    private float lastVelocity;

    //Debug
    //private Vector2 _origin;
    //private Vector2 _targetPoint;
    //private Vector2 _hitPoint;

    void FixedUpdate()
    {
        if(PlayerRigidBody == null)
        {
            return;
        }

        var velocity = PlayerRigidBody.velocity.x;

        //Filter through the ContactPoints to see if we're grounded and to see if we can step up
        ContactPoint2D groundCP = default(ContactPoint2D);
        bool grounded = FindGround(out groundCP, allCPs);

        Vector2 stepUpOffset = default(Vector2);
        bool stepUp = false;
        if (grounded)
            stepUp = FindStep(out stepUpOffset, allCPs, groundCP, velocity);

        //Steps
        if (stepUp)
        {            
            PlayerRigidBody.position += stepUpOffset;
            PlayerRigidBody.velocity = new Vector2(lastVelocity, PlayerRigidBody.velocity.y);
        }

        allCPs.Clear();
        lastVelocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        ContactPoint2D[] contact = new ContactPoint2D[col.contactCount];
        col.GetContacts(contact);
        allCPs.AddRange(contact);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        ContactPoint2D[] contact = new ContactPoint2D[col.contactCount];
        col.GetContacts(contact);
        allCPs.AddRange(contact);
    }

    /// Finds the MOST grounded (flattest y component) ContactPoint2D
    /// \param allCPs List to search
    /// \param groundCP The contact point with the ground
    /// \return If grounded
    bool FindGround(out ContactPoint2D groundCP, List<ContactPoint2D> allCPs)
    {
        groundCP = default(ContactPoint2D);
        bool found = false;
        foreach (ContactPoint2D cp in allCPs)
        {
            //Pointing with some up direction
            if (cp.normal.y > 0.0001f && (found == false || cp.normal.y > groundCP.normal.y))
            {
                groundCP = cp;
                found = true;
            }
        }

        return found;
    }

    /// Find the first step up point if we hit a step
    /// \param allCPs List to search
    /// \param stepUpOffset A Vector2 of the offset of the player to step up the step
    /// \return If we found a step
    bool FindStep(out Vector2 stepUpOffset, List<ContactPoint2D> allCPs, ContactPoint2D groundCP, float currVelocity)
    {
        stepUpOffset = default(Vector2);

        //No chance to step if the player is not moving
        if (InputHandler.instance.MoveInput.x ==0)
            return false;

        foreach (ContactPoint2D cp in allCPs)
        {
            bool test = ResolveStepUp(out stepUpOffset, cp, groundCP);
            if (test)
                return test;
        }
        return false;
    }

    /// Takes a contact point that looks as though it's the side face of a step and sees if we can climb it
    /// \param stepTestCP ContactPoint2D to check.
    /// \param groundCP ContactPoint2D on the ground.
    /// \param stepUpOffset The offset from the stepTestCP.point to the stepUpPoint (to add to the player's position so they're now on the step)
    /// \return If the passed ContactPoint2D was a step
    bool ResolveStepUp(out Vector2 stepUpOffset, ContactPoint2D stepTestCP, ContactPoint2D groundCP)
    {
        stepUpOffset = default(Vector2);
        Collider2D stepCol = stepTestCP.otherCollider;

        //( 1 ) Check if the contact point normal matches that of a step (y close to 0)
        if (Mathf.Abs(stepTestCP.normal.y) >= 0.01f)
        {
            return false;
        }

        //( 2 ) Make sure the contact point is low enough to be a step
        if (!(stepTestCP.point.y - groundCP.point.y < maxStepHeight))
        {
            return false;
        }

        //( 3 ) Check to see if there's actually a place to step in front of us
        //Fires one Raycast
        float stepHeight = groundCP.point.y + maxStepHeight + 0.001f;
        Vector2 stepTestInvDir = new Vector2(-stepTestCP.normal.x, 0).normalized;
        Vector2 origin = new Vector2(stepTestCP.point.x, stepHeight) + (stepTestInvDir * stepSearchOvershoot);
        Vector2 direction = Vector2.down;

        //_origin = origin;
        //_targetPoint = origin + direction;

        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, maxStepHeight + 0.1f, _stepLayer.value);
        if (hitInfo.collider == null)
        {
            return false;
        }

        //_hitPoint = hitInfo.point;

        //We have enough info to calculate the points
        Vector2 stepUpPoint = new Vector2(stepTestCP.point.x, hitInfo.point.y + 0.0001f) + (stepTestInvDir * stepSearchOvershoot);
        Vector2 stepUpPointOffset = stepUpPoint - new Vector2(stepTestCP.point.x, groundCP.point.y);

        
        
        //We passed all the checks! Calculate and return the point!
        stepUpOffset = stepUpPointOffset;


        return true;
    }

    //void OnDrawGizmos()
    //{
    //    if (_origin == null || _targetPoint == null)
    //    {
    //        return;
    //    }

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(_origin, 0.05f);

    //    if (_hitPoint != null)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawSphere(_hitPoint, 0.02f);
    //    }

    //    Gizmos.color = Color.black;
    //    //Gizmos.DrawLine(_origin, _targetPoint);
    //    Gizmos.DrawRay(new Ray(_origin, Vector2.down));
    //}
}
