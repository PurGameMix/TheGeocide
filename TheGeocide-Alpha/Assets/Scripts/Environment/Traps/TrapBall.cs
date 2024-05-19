using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

public class TrapBall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private GroundedFire _GroundedFirePrefab;

    [SerializeField]
    private AudioController _audioController;

    [SerializeField]
    LayerMask _fireMask;

    [SerializeField]
    private LayerMask _canTriggerFire;

    private bool _isReleased = false;
    private float _fireDropRate = 0.5f;
    private float _lastBurnTime = 0f;
    private float _flameHeight;


    void OnCollisionStay2D(Collision2D collision)
    {
        if (_canTriggerFire.Contains(collision.gameObject.layer))
        {
            ContactPoint2D[] contactList = new ContactPoint2D[collision.contactCount];

            if(collision.contactCount == 0)
            {
                return;
            }
            collision.GetContacts(contactList);
            PutFlame(contactList[0]);
        }
    }

    private void PutFlame(ContactPoint2D contactPoint2D)
    {
        if(_lastBurnTime > 0)
        {
            return;
        }


        var collider = Physics2D.OverlapBox(contactPoint2D.point, new Vector2(1, 1), 0, _fireMask);
        if (collider != null)
        {
            //Debug.Log("Already a flame");
            return;
        }

        _lastBurnTime = _fireDropRate;
        var fire = Instantiate(_GroundedFirePrefab);

        var direction = Quaternion.FromToRotation(Vector2.up, contactPoint2D.normal);
        fire.transform.rotation = new Quaternion(0, 0, direction.z , direction.w);
        fire.transform.position = new Vector2(contactPoint2D.point.x, contactPoint2D.point.y + _flameHeight);
    }

    void Start()
    {      
        _flameHeight = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isReleased)
        {
            return;
        }

        _lastBurnTime -= Time.deltaTime;
    }

    public void Release()
    {
        _isReleased = true;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _audioController.Play("Move");
    }
}
