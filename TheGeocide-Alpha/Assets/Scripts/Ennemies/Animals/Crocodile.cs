using Assets.Data.Enemy.Definition;
using Assets.Scripts.Ennemies.Animals;
using System;
using UnityEngine;

public class Crocodile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private AudioController _audioController;
    [SerializeField]
    private EnemyThreatController _threatController;
    [SerializeField]
    private bool _isDrawingFacingRight;
    private bool _isRotate;

    [Range(0, .3f)]
    [SerializeField]
    private float m_MovementSmoothing = .05f;
    private float _baseSpeed = 25f;
    private float _chaseSpeed = 50f;
    private Vector2 m_Velocity = Vector3.zero;

    private CrocodileState _currentState;

    [SerializeField]
    private Transform _attackPoint;
    private int _attackDamage = 1000;
    private float _attackRange = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        _currentState = CrocodileState.Idle;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(_currentState == CrocodileState.Despawn)
        {
            return;
        }

        if (_threatController.PlayerInStrictSuspeciousRange() && _currentState != CrocodileState.Move)
        {
            _currentState = CrocodileState.Move;
            _animator.Play("Swim");
            return;
        }

        if (_threatController.PlayerInStrictAggroRange() && _currentState != CrocodileState.MoveToAttack)
        {
            _currentState = CrocodileState.MoveToAttack;
            _animator.Play("SwimToAttack");
            return;
        }

        if (_threatController.EntityInAttackRange() && _currentState != CrocodileState.Attack)
        {
            _currentState = CrocodileState.Attack;
            Attack();
            return;
        }
    }

    internal void Despawn()
    {
        _currentState = CrocodileState.Despawn;
        _animator.Play("Despawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState == CrocodileState.Move)
        {
            Debug.Log("MoveToPlayer");
            MoveToPlayer();
            return;
        }

        if (_currentState == CrocodileState.MoveToAttack)
        {
            Debug.Log("MoveToAttack");
            MoveToAttack();
            return;
        }
    }

    private void MoveToPlayer()
    {
        var target = _threatController.GetFocusTarget(ThreatDetectionType.Suspecious);
        if (target == null)
        {
            return;
        }
        MoveCrocodile(target.position, _baseSpeed);
    }

    private void MoveToAttack()
    {
        var target = _threatController.GetFocusTarget(ThreatDetectionType.Aggressive);
        if(target == null)
        {
            return;
        }
        MoveCrocodile(target.position, _chaseSpeed);
    }

    private void MoveCrocodile(Vector3 targetPos, float targetSpeed)
    {
        var difference = targetPos.x - transform.position.x < 0 ? -1 : 1;
        targetSpeed = targetSpeed * difference;
        float accelRate= (Mathf.Abs(targetSpeed) > 0.01f) ? 10 : 20;

        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = 1.23f;
        }
        else if (Mathf.Abs(_rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(_rb.velocity.x)))
        {
            velPower = 1.13f;
        }
        else
        {
            velPower = 1.05f;
        }

        float speedDif = targetSpeed - _rb.velocity.x; //calculate difference between current velocity and desired velocity
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        var applyForce= Mathf.Lerp(_rb.velocity.x, movement, m_MovementSmoothing);
        _rb.AddForce(applyForce * Vector2.right);
        FacePlayerDirection(targetPos);
    }

    private void Attack()
    {
        var target = _threatController.GetFocusTarget(ThreatDetectionType.Attack);
        if (target == null)
        {
            return;
        }
        FacePlayerDirection(target.position);
        _animator.Play("Attack");
        _audioController.Play("SwimAttack");
    }

    private void FacePlayerDirection(Vector2 target)
    {
        var playerLeft = transform.position.x - target.x > 0;

        if(_isDrawingFacingRight && !_isRotate && playerLeft)
        {
            _isRotate = true;
            _animator.gameObject.transform.Rotate(0,180,0);
            return;
        }

        if (!_isDrawingFacingRight && _isRotate && playerLeft)
        {
            _isRotate = false;
            _animator.gameObject.transform.Rotate(0, 180, 0);
            return;
        }

        if (_isDrawingFacingRight && _isRotate && !playerLeft)
        {
            _isRotate = false;
            _animator.gameObject.transform.Rotate(0, 180, 0);
            return;
        }

        if (!_isDrawingFacingRight && !_isRotate && !playerLeft)
        {
            _isRotate = true;
            _animator.gameObject.transform.Rotate(0, 180, 0);
            return;
        }
    }

    public void DoDammage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange);

        foreach (Collider2D collider in hitEnemies)
        {
            if (collider.isTrigger)
            {
                continue;
            }

            if (collider.gameObject.name == name)
            {
                continue;
            }

            var canBeDamage = collider.GetComponent<ICanBeDamaged>();

            if (canBeDamage != null)
            {
                _audioController.Play("Hit");
                canBeDamage.TakeDamage(_attackDamage, HealthEffectorType.enemy);
            }
        }
    }

    public void AttackCompleted() {
        _currentState = CrocodileState.Idle;
        _animator.Play("Idle");
    }

    public void DespawnCompleted()
    {
        Destroy(gameObject);
    }

    public void SwimLight()
    {
        var rng = UnityEngine.Random.Range(1, 3);
        _audioController.Play("SwimLight" + rng);
    }

    public void SwimHeavy()
    {
        var rng = UnityEngine.Random.Range(1, 3);
        _audioController.Play("SwimHeavy" + rng);
    }

    private void OnDrawGizmos()
    {
        if (_attackPoint != null)
        {
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
        }
    }
}
