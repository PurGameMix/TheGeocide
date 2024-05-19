using Assets.Data.Common.Definition;
using Assets.Scripts.Utils;
using UnityEngine;

public class MovingRopePlateform : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private BoxCollider2D _boxCollider;

    private TargetJoint2D _objectJoint;
    private PlayerStateMachine _playerSt;

    private float _xDiff;
    public float _yDiff = 0.6f;
    public bool IsBreak;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsBreak)
        {
            return;
        }

        var hit = BoxCollideSideUtils.GetHitSide(_boxCollider, collision.collider);

        if(hit != CollisionSide2D.Top)
        {
            return;
        }

        var playerSt = collision.gameObject.GetComponent<PlayerStateMachine>();
        if (playerSt != null)
        {
            _playerSt = playerSt;
            _objectJoint = collision.gameObject.GetComponent<TargetJoint2D>();
            _xDiff = _objectJoint.transform.position.x - transform.position.x;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && _playerSt != null)
        {
            _objectJoint.enabled = false;
            _objectJoint = null;
            _playerSt = null;
        }
    }

    private void Update()
    {

        if(IsBreak && _rb.gravityScale == 1)
        {
            _rb.gravityScale = 10;
            _rb.angularDrag = 10;
            _rb.drag = 10;
        }

        if(_playerSt == null)
        {
            return;
        }

        if (IsBreak)
        {
            _objectJoint.enabled = false;
            _objectJoint = null;
            _playerSt = null;
            return;
        }

        if (_playerSt.CurrentState == "PlayerIdleState")
        {
            _objectJoint.enabled = true;
            _objectJoint.target = new Vector2(transform.position.x + _xDiff, transform.position.y + _yDiff);
        }
        else
        {
            _objectJoint.enabled = false;
            _xDiff = _objectJoint.transform.position.x - transform.position.x;
        }
    }

    //void OnDrawGizmos()
    //{

    //#if UNITY_EDITOR
    //        Gizmos.color = Color.yellow;
    //    if(_playerSt != null)
    //    {
    //        Gizmos.DrawSphere(new Vector2(transform.position.x + _xDiff, transform.position.y + _yDiff), 0.1f);
    //    }
    //#endif
    //}
}
