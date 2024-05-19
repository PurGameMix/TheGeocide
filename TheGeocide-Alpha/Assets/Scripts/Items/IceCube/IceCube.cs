using System;
using UnityEngine;

public class IceCube : InvocationFX, ICanBeDamaged
{
    public float KickPower;
    public float SpeedLimitMax = 8f;
    public float SpeedLimitMin = 1f;
    public float MeltTime = 10f;
    private bool _isMelting;
    private float _currentMeltTime;

    public float FloatingTime = 1f;
    private float _timeFloating = 0f;
    private bool _startFloatingCD;

    [SerializeField]
    private SummonSpell _mainSpell;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private Collider2D _col;

    [SerializeField]
    private SpriteRenderer _sr;

    [SerializeField]
    private Sprite _boostedSprite;

    [SerializeField]
    private AudioController _audioController;

    private bool _isBoosted;
    private Vector2 _velocityBeforePhysicUpdate;

    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;

    public void TakeDamage(int damage, HealthEffectorType type)
    {
        if (type == HealthEffectorType.enemy || type == HealthEffectorType.player)
        {
            Break();
            return;
        }

        if (type.IsFire())
        {
            Melt();
            return;
        }

        if (type == HealthEffectorType.playerFroze)
        {
            HandleFrozenBoost();
            return;
        }

        if (type == HealthEffectorType.playerFrozeFist)
        {
            HandleFrozenBoost();
            _rb.isKinematic = false;
            _rb.AddForce(new Vector2(0, - _rb.mass* KickPower), ForceMode2D.Impulse);
            return;
        }
    }

    private void HandleFrozenBoost()
    {
        _audioController.Play("HeavyHit");
        _isBoosted = true;
        _sr.sprite = _boostedSprite;
        _currentMeltTime = 0;
    }

    private void Melt()
    {
        _isMelting = true;
        _mainSpell.UnSummon();
    }

    private void Break()
    {
        _mainSpell.Break(_isBoosted, _isGrounded);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.isTrigger || col.gameObject.tag == "Player")
        {
            return;
        }

        var ySpeed = Math.Abs(_velocityBeforePhysicUpdate.y);

        if (ySpeed < SpeedLimitMin)
        {
            return;
        }

        if (ySpeed < SpeedLimitMax)
        {
            _audioController.Play("LightHit");
            return;
        }

        //Debug.Log("Break speed :" + ySpeed);
        _audioController.Play("HeavyHit");
        Break();


    }


    private void FixedUpdate()
    {
        if (!_isGrounded)
        {
            _velocityBeforePhysicUpdate = _rb.velocity;
        }
    }
    private void Update()
    {
        var test  = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);
        _isGrounded = test != null && test != _col;

        _currentMeltTime += Time.deltaTime;
        if (!_isMelting && _currentMeltTime >= MeltTime)
        {
            Melt();
            return;
        }
        if (_startFloatingCD && _rb.isKinematic)
        {
            _timeFloating += Time.deltaTime;
            if (_timeFloating >= FloatingTime)
            {
                _rb.isKinematic = false;
            }
        }
    }

    internal override void Init(Action onCompleted = null)
    {
        _startFloatingCD = true;
        _sr.enabled = true;
    }

    internal override void Destroy(Action onCompleted = null)
    {
        _sr.enabled = false;
    }

    internal override void EffectCompleted(string effectName)
    {
        throw new NotImplementedException();
    }

    void OnDrawGizmos()
    {

#if UNITY_EDITOR
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(_groundCheckPoint.transform.position, _groundCheckSize);
#endif
    }

    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }
}
