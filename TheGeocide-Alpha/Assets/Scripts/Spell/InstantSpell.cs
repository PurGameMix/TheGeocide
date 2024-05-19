using Assets.Scripts.Spell.Entity;
using Assets.Scripts.Utils;
using System;
using UnityEngine;

/// <summary>
/// A spell In/Out after animation completed
/// Usable for player or enemies
/// </summary>
public class InstantSpell : MonoBehaviour
{
    public bool IsEnemySpell;
    public float RepulseForce;
    private Transform _repulseOrigin;
    [SerializeField]
    private Collider2D _collider2D;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private LayerMask _hitLayers;

    internal int _damage = 34;


    private float _effectProbability;
    private HealthEffectorType _effectType;

    private void Start()
    {
    }

    void HandleHitStart()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(_collider2D.bounds.center, _collider2D.bounds.size, 0,_hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            HandleSpellDamage(collider);
        }
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (!_hitLayers.Contains(hitInfo.gameObject.layer))
        {
            return;
        }
        HandleSpellDamage(hitInfo);
    }

    public virtual void DestroySpell()
    {
        Destroy(gameObject);
    }


    /// <summary>
    /// Spell from player
    /// </summary>
    /// <param name="item"></param>
    public void Init(PlayerItemSO item)
    {
        _damage = item.Damage;
        _effectProbability = item.EffectProbability;
        _effectType = item.EffectType;
        _animator.Play("Spell");
        HandleHitStart();
    }

    /// <summary>
    /// Spell from enemies TODO: use EnemyWeaponDataSo?
    /// </summary>
    /// <param name="item"></param>
    public void Init(EnemySpellInfos esInfos)
    {
        _damage = esInfos.Damage;
        RepulseForce = esInfos.RepulseForce;
        _repulseOrigin = esInfos.RepulseOrigin != null? esInfos.RepulseOrigin : transform;
        _effectProbability = esInfos.EffecProbability;
        _effectType = esInfos.Effector;
        _animator.Play("Spell");
        HandleHitStart();
    }

    private void HandleSpellDamage(Collider2D hitInfo)
    {
        var canBeDamageObj = hitInfo.GetComponent<ICanBeDamaged>();
        if (canBeDamageObj != null)
        {
            canBeDamageObj.TakeDamage(_damage, GetEffector());
        }

        if (RepulseForce <= 0)
        {
            return;
        }

        var repulsable = hitInfo.GetComponent<ICanBeRepulsed>();
        if (repulsable != null)
        {
            repulsable.TakeKnockBack(GetRepulseMagnitude(hitInfo.transform,RepulseForce));
        }
    }

    private Vector2 GetRepulseMagnitude(Transform target, float repulseForce)
    {
        var direction = (target.position - _repulseOrigin.position).normalized;
        return new Vector2(direction.x * repulseForce, 0);
    }

    private bool isPushingLeft(Transform target)
    {
        return _repulseOrigin.position.x - target.position.x > 0;
    }

    private HealthEffectorType GetEffector()
    {
        if(_effectProbability == 100)
        {
            return _effectType;
        }

        var rng = UnityEngine.Random.Range(1, 101);
        if(rng <= _effectProbability)
        {
            return _effectType;
        }

        return IsEnemySpell? HealthEffectorType.enemy :  HealthEffectorType.player; //no effector
    }
}
