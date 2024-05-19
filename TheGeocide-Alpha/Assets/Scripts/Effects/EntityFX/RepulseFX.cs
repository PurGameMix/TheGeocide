using System;
using UnityEngine;

public class RepulseFX : LoopFX
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private AudioSource _audio;

    public override void StartFX()
    {
        _animator.Play("Effect");
        if(_audio != null)
        {
            _audio.Play();
        }
    }

    public override void StopFX()
    {
        _animator.Play("Idle");
        if (_audio != null)
        {
            _audio.Stop();
        }      
    }

    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }

    internal override void EffectCompleted(string effectName)
    {
        throw new NotImplementedException();
    }
}
