using System;
using UnityEngine;

public class ElecMine : MonoBehaviour
{
    public float BombTime = 10f;
    private float _currentTime;
    private bool _isExploding;

    public float FloatingTime = 0.2f;
    private float _timeFloating = 0f;
    private bool _isStartFloating;

    [SerializeField]
    private SummonSpell _mainSpell;

    [SerializeField]
    private ElecMineTrigger _triger;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private Collider2D _col;

    [SerializeField]
    private AudioController _audioController;

    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;
    private bool _firstTimeGrounded = true;
    private void BombExplode()
    {
        _isExploding = true;
        _mainSpell.Break(false, _isGrounded);
        _triger.Destroy();
    }
    private void Start()
    {
        _isStartFloating = true;
    }

    private void Update()
    {
        var test  = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);
        _isGrounded = test != null && test != _col;

        if(_isGrounded && _firstTimeGrounded)
        {
            _firstTimeGrounded = false;
            _triger.Init();
        }
        _currentTime += Time.deltaTime;
        if (!_isExploding && _currentTime >= BombTime)
        {
            BombExplode();
            return;
        }

        if (_isStartFloating && _rb.isKinematic)
        {
            _timeFloating += Time.deltaTime;
            if (_timeFloating >= FloatingTime)
            {
                _rb.isKinematic = false;
            }
        }
    }

    void OnDrawGizmos()
    {

#if UNITY_EDITOR
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(_groundCheckPoint.transform.position, _groundCheckSize);
#endif
    }

}
