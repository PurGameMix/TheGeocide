using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public float SoundVelocityLimit = 5;

    [SerializeField]
    private AudioController _audioController;

    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    public float LinearDragX;

    private float _linearDragX;

    [SerializeField]
    private LayerMask _canImpactLayer;

    [SerializeField]
    private LayerMask _movableLayer;
    [SerializeField]
    private bool _isSlave;
    private bool _isMovingH;
    private bool _isPlaying;
    private bool _isFallingHard;
    private float _movingMargin = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        _linearDragX = LinearDragX;
    }

    // Update is called once per frame
    void Update()
    {
        
        _isMovingH = Mathf.Abs(_rb.velocity.x) > _movingMargin && Mathf.Abs(_rb.velocity.y) < _movingMargin;

        _isFallingHard = Mathf.Abs(_rb.velocity.y) > SoundVelocityLimit;       
    }

    void FixedUpdate()
    {
        if (_isMovingH && !_isPlaying)
        {
            //Debug.Log("Moving");
            _isPlaying = true;
            _audioController.Play("Move");
            return;
        }

        if (!_isMovingH && _isPlaying)
        {
            _isPlaying = false;
            _audioController.Stop("Move");
        }

        _rb.AddForce(new Vector2(-(_rb.velocity.x * _linearDragX), 0));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isFallingHard && _canImpactLayer.Contains(collision.gameObject.layer) )
        {
            _audioController.Play("Impact");
        }

        if (_movableLayer.Contains(collision.gameObject.layer))
        {
            if (_isSlave)
            {
                _linearDragX = 0;
                _rb.freezeRotation = false;
            }
            else
            {
                collision.transform.SetParent(transform);
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_movableLayer.Contains(collision.gameObject.layer))
        {
            if (_isSlave)
            {
                _linearDragX = LinearDragX;
                _rb.freezeRotation = true;
            }
            else
            {
                collision.transform.SetParent(null);
            }
        }
    }


}
