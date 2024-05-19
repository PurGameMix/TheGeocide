using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;

/// <summary>
/// Todo poison snake?
/// </summary>
public class Snake : MonoBehaviour
{

    public int Damage = 20;

    [SerializeField]
    private Animator _animator;
    
    [SerializeField]
    private AudioController _audioController;
    [SerializeField]
    private Transform _attackPoint;

    [SerializeField]
    private LayerMask _hitLayers;

    private float attackRange = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        //Play IdleSound a litlle randomly
        StartCoroutine(PlayIdleSound());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hitLayers.Contains(collision.gameObject.layer))
        {
            _animator.Play("Snake_Attack");
            _audioController.Stop("SnakeIdle");
            _audioController.Play("SnakeAttack");
        }
    }

    private IEnumerator PlayIdleSound()
    {
        var random = Random.Range(0, 10);

        yield return new WaitForSeconds(random);
        _audioController.Play("SnakeIdle");
    }

    public void SnakeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, attackRange, _hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            var canBeDamage = collider.GetComponent<ICanBeDamaged>();
            if (canBeDamage != null)
            {
                canBeDamage.TakeDamage(Damage, HealthEffectorType.trap);
                _audioController.Play("SnakeHit");
            }
        }
    }

    public void SnakeAttackCompleted()
    {
        _audioController.Play("SnakeIdle");
    }

    private void OnDrawGizmosSelected()
    {

        if (_attackPoint != null)
        {
            Gizmos.DrawWireSphere(_attackPoint.position, attackRange);
        }

    }
}
