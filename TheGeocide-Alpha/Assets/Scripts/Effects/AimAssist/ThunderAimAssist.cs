using System;
using UnityEngine;

public class ThunderAimAssist : AimAssist
{

    private float _currentTime = 0f;
    public float AimTime;
    public float AimVelocity;
    public Rigidbody2D _rb;
    public SpriteRenderer _sr;
    public Color cancelColor;

    [Header("Sky check")]
    [SerializeField]
    private Transform _checkSkyPoint;
    [SerializeField]
    private LayerMask _cancelLayers;
    public float maxSkyHeightCheck;

    private Transform _cursor;

    [SerializeField]
    private AudioController _audioController;

    private bool _isClearToShoot = false;
    private bool _isStopped = false;

    // Start is called before the first frame update
    void Start()
    {

        _rb.velocity = Vector2.right * AimVelocity;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        
        if(_currentTime > AimTime)
        {
            StopMarker();
            return;
        }

        if(_cursor.position.x < _rb.position.x)
        {
            _rb.velocity = Vector2.left * AimVelocity;
        }

        if (_cursor.position.x > _rb.position.x)
        {
            _rb.velocity = Vector2.right * AimVelocity;
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        RaycastHit2D hitInfo = Physics2D.Raycast(_checkSkyPoint.position, Vector2.up, maxSkyHeightCheck, _cancelLayers.value);

        _isClearToShoot = hitInfo.collider == null;

        if (_isClearToShoot)
        {
            _sr.color = Color.white;
            _audioController.SetVolume("Charging", 1f);
            _audioController.SetVolume("ChargedLoop", 1f);
        }
        else
        {
            _sr.color = cancelColor;
            _audioController.SetVolume("Charging", 0.5f);
            _audioController.SetVolume("ChargedLoop", 0.5f);
        }
    }

    void StopMarker()
    {
        if (_isStopped)
        {
            return;
        }
        _rb.velocity = Vector2.zero;
        _audioController.Stop("Charging");
        _audioController.Play("ChargedLoop");
        _isStopped = true;
    }

    public override void SetCursor(Transform cursorTransform)
    {
        _cursor = cursorTransform;
    }

    public override void Stop()
    {
        StopMarker();
    }

    public override float GetAimWindow()
    {
        return 0;
    }

    public override Transform GetPosition()
    {
        if (_isClearToShoot)
        {
            return _rb.transform;
        }

        return null;
    }

    public override void SetCastTime(float castTime)
    {
        AimTime = castTime;
    }
}
