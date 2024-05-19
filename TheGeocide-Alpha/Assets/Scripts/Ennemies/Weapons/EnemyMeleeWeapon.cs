using System;
using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{
    public Transform attackPoint;
    [SerializeField]
    private Transform _repulsePoint;

    [SerializeField]
    private LayerMask _hitLayers;

    private int _attackDamage = 34;
    public float attackRange = 0.3f;

    private float _repulseForce = 10f;
    private Vector2 _repulseRange = new Vector2(0.3f,0.05f);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    //Called by animator
    /// <summary>
    /// Damage objects in weapon range
    /// </summary>
    /// <returns>True one firt hit</returns>
    public bool MeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, _hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            if (collider.gameObject.name == name)
            {
                continue;
            }

            var canBeDamagedEntity = collider.GetComponent<ICanBeDamaged>();

            if(canBeDamagedEntity != null)
            {
                canBeDamagedEntity.TakeDamage(_attackDamage, HealthEffectorType.enemy);
                return true;
            }
        }
        return false;
    }

    //Called by animator
    /// <summary>
    /// Repulse objects in close range
    /// </summary>
    /// <returns>True one fisrt hit</returns>
    internal bool RepulseAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(_repulsePoint.position, _repulseRange, 0, _hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            if (collider.gameObject.name == name)
            {
                continue;
            }

            var repulsable = collider.GetComponent<ICanBeRepulsed>();

            if (repulsable != null)
            {
                repulsable.TakeKnockBack(GetRepulseMagnitude(collider.transform, _repulseForce));
                return true;
            }
        }

        return false;
    }

    private Vector2 GetRepulseMagnitude(Transform target, float repulseForce)
    {
        var direction = (_repulsePoint.position - target.position).normalized;
        return new Vector2(direction.x * repulseForce, 0);
    }

    //Called by animator
    /// <summary>
    /// Damage & Repulse objects in weapon range
    /// </summary>
    /// <returns>True one fisrt hit</returns>
    internal bool ChargeAttack(bool isLookingLeft)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

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

            var player = collider.GetComponent<ICanBeDamaged>();

            var hit = false;
            if (player != null)
            {
                player.TakeDamage(_attackDamage * 2, HealthEffectorType.enemy);
                hit = true;
            }

            var repuslable = collider.GetComponent<ICanBeRepulsed>();
            if (repuslable != null)
            {
                repuslable.TakeKnockBack(GetRepulseMagnitude(collider.transform, _repulseForce/2));
                hit = true;
            }

            if (hit)
            {
                return true;
            }
            
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        if (_repulsePoint != null)
        {

            Gizmos.DrawWireCube(_repulsePoint.position, _repulseRange);
        }

    }
}
