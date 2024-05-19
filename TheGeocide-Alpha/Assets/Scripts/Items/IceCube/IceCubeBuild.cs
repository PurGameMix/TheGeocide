using System;
using UnityEngine;

public class IceCubeBuild : InitionFX
{
    [SerializeField]
    internal Animator _animator;
    [SerializeField]
    private Collider2D _col;

    [SerializeField]
    internal AudioController _audioController;
    private Action _emergeCompleted;
    private Action _destroyCompleted;

    public void Emerge()
    {
        _animator.Play("iceCube_build");
        _audioController.Play("Emerge");
    }

    public void Melt()
    {
        _audioController.Play("Melt");
        _animator.Play("iceCube_melt");
    }


    public void EmergeCompleted()
    {
        _animator.Play("Idle");
        if(_emergeCompleted != null)
        {
            _emergeCompleted();
        }      
    }

    public void MeltCompleted()
    {
        _animator.Play("Idle");
        if (_destroyCompleted != null)
        {
            _destroyCompleted();
        }
    }

    internal override void Init(Action onCompleted = null)
    {
        _col.enabled = true;
        Emerge();
        _emergeCompleted = onCompleted;
    }

    internal override void Destroy(Action onCompleted = null)
    {
        _destroyCompleted = onCompleted;
        Melt();
    }

    internal override void EffectCompleted(string effectName)
    {
        throw new NotImplementedException();
    }

    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }
}
