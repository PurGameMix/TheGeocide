using System;
using UnityEngine;

public class ElecMineTrigger : ActionFX
{
    [SerializeField]
    private SummonSpell _mainSpell;

    [SerializeField]
    private AudioController _audioController;

    [SerializeField]
    private Collider2D _collider2D;

    [SerializeField]
    private LayerMask _hitLayers;

    [SerializeField]
    private Animator _auraAnimator;
    [SerializeField]
    private Animator _electricWireAnimator;

    public void HandleDetection()
    {

        _audioController.Play("Triggers");
        var hitEnemies = Physics2D.OverlapBoxAll(_collider2D.bounds.center, _collider2D.bounds.size, 0, _hitLayers);
        if(hitEnemies.Length > 0)
        {
            _mainSpell.Break(false, false);
        }
    }

    internal override void ActionTriggered(string effectName)
    {
        if(effectName == _electricWireAnimator.gameObject.name)
        {
            HandleDetection();
        }

        if (effectName == _auraAnimator.gameObject.name)
        {
            _audioController.Play("Arc");
        }
        
    }

    internal void Destroy()
    {
        _audioController.Stop("Arc");
        _audioController.Stop("Triggers");

        _auraAnimator.Play("Idle");
        _electricWireAnimator.Play("Idle");
    }

    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }

    internal override void EffectCompleted(string effectName)
    {
        throw new NotImplementedException();
    }

    internal void Init()
    {
        _auraAnimator.Play("Effect");
        _electricWireAnimator.Play("Effect");
    }
}
