using System;
using UnityEngine;

public class ElecExplosion : UnSummonFX
{
    [SerializeField]
    private AudioSource _audio;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Collider2D _collider2D;
    [SerializeField]
    private LayerMask _hitLayers;

    private int _damage = 30;
    private Action _onEffectCompleted;
    public void HandleExplosion()
    {
        var hitEnemies = Physics2D.OverlapBoxAll(_collider2D.bounds.center, _collider2D.bounds.size, 0, _hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            HandleSpellDamage(collider);
        }
    }

    public void AnimationCompleted()
    {
        Destroy(gameObject);
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

    private HealthEffectorType GetEffector()
    {
        return HealthEffectorType.playerElec;
    }

    internal override void SetParameters(bool isBoosted = false, bool isGrounded = false)
    {
    }

    internal override void Init(Action onCompleted = null)
    {
        _animator.Play("Effect");
        _audio.Play();
        _onEffectCompleted = onCompleted;
    }

    internal override void Destroy(Action onCompleted = null)
    {
        throw new NotImplementedException();
    }

    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }

    internal override void EffectCompleted(string effectName)
    {
        if (_onEffectCompleted != null)
        {
            _onEffectCompleted();
        }
    }
}
