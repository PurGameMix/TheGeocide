using System;
using UnityEngine;

public class ElecBall : ActionFX
{
    [SerializeField]
    private AudioController _audioController;
    [SerializeField]
    private Orbit _orbit;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Collider2D _collider2D;
    [SerializeField]
    private LayerMask _hitLayers;

    private int _damage = 30;

    private HealthEffectorType GetEffector()
    {
        return HealthEffectorType.playerElec;
    }

    private void Start()
    {
       
    }

    internal void OnOrbitReached()
    {
        _audioController.Play("Idle");
    }
    internal void Init(int dmg)
    {
        _damage = dmg;
        _animator.Play("IdleVisible");
        _orbit.Start(OnOrbitReached);
        //_audioController.Play("Idle");
    }

    internal override void EffectBegin(string effectName)
    {
        _audioController.Play("Explode");
    }

    internal override void ActionTriggered(string effectName)
    {
        HandleExplosion();
    }

    internal override void EffectCompleted(string effectName)
    {
        Destroy(gameObject);
    }

    private void HandleExplosion()
    {
        _audioController.Stop("Idle");

        var hitEnemies = Physics2D.OverlapBoxAll(_collider2D.bounds.center, _collider2D.bounds.size, 0, _hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            HandleSpellDamage(collider);
        }
    }
    private void HandleSpellDamage(Collider2D hitInfo)
    {

        if (hitInfo.isTrigger)
        {
            return;
        }

        if (hitInfo.tag == "Player")
        {
            return;
        }

        var canBeDamageObj = hitInfo.GetComponent<ICanBeDamaged>();
        if (canBeDamageObj != null)
        {

            canBeDamageObj.TakeDamage(_damage, GetEffector());
        }
    }
}
