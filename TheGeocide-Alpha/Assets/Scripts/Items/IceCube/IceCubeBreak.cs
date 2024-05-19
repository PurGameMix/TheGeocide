using System;
using UnityEngine;

public class IceCubeBreak : UnSummonFX
{
    public bool IsBoosted;

    [SerializeField]
    internal Animator _animator;

    [SerializeField]
    internal AudioSource _breakSound;

    [SerializeField]
    internal GameObject _iceExplosionPrefab;
    [SerializeField]
    internal GameObject _iceSpikePrefab;

    [SerializeField]
    internal Transform _explodePoint;
    [SerializeField]
    internal Transform _spikePoint;

    private bool _isBoosted;
    private bool _isGrounded;
    private Action _onEffectCompleted;


    public void ExplosionBegin()
    {
       
        if (_isBoosted)
        {
            Instantiate(_iceExplosionPrefab, _explodePoint.position, _explodePoint.rotation);

            if (_isGrounded)
            {
                Instantiate(_iceSpikePrefab, _spikePoint.position, _spikePoint.rotation);
            }
        }
    }
    internal override void Init(Action onCompleted = null)
    {
        _onEffectCompleted = onCompleted;
        _animator.Play("iceCube_break");
        _breakSound.Play();
    }

    internal override void SetParameters(bool isBoosted, bool isGrounded)
    {
        _isBoosted = isBoosted;
        _isGrounded = isGrounded;
    }

    internal override void Destroy(Action onCompleted = null)
    {
    }

    internal override void EffectCompleted(string effectName)
    {
        if(_onEffectCompleted != null)
        {
            _onEffectCompleted();
        }    
    }

    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }
}
