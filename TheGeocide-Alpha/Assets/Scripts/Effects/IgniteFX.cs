using System;
using UnityEngine;

public class IgniteFX : TimerEntityFX
{
    [SerializeField]
    private SubEffectFX _raiseFx;
    private SubEffectFX _raiseFxInstance;

    [SerializeField]
    private SubEffectFX _loopFx;
    private SubEffectFX _loopFxInstance;

    [SerializeField]
    private SubEffectFX _downFx;
    private SubEffectFX _downFxInstance;

    private float _IgniteTime;
    private float _currentFxTime;
    private bool _isActiveCD;

    void Start()
    {
        //_raiseFx.SetMasterEffect(this);
        //_loopFx.SetMasterEffect(this);
        //_downFx.SetMasterEffect(this);
    }

    void FixedUpdate()
    {
        if (!_isActiveCD)
        {
            return;
        }
        _currentFxTime += Time.deltaTime;
    }

    public override void StartFX(float fxTime)
    {
        _IgniteTime = fxTime;
        _raiseFxInstance = Instantiate(_raiseFx, transform);
        _raiseFxInstance.SetMasterEffect(this);
    }

    internal override void EffectCompleted(string effectName)
    {
        if(_raiseFxInstance?.name == effectName)
        {
            Destroy(_raiseFxInstance.gameObject);
            _raiseFxInstance = null;
            _loopFxInstance = Instantiate(_loopFx, transform);
            _loopFxInstance.SetMasterEffect(this);
            _isActiveCD = true;
            return;
        }

        if (_loopFxInstance?.name == effectName && IsIgniteOver())
        {
            Destroy(_loopFxInstance.gameObject);
            _loopFxInstance = null;
            _downFxInstance = Instantiate(_downFx, transform);
            _downFxInstance.SetMasterEffect(this);
            _isActiveCD = false;
            return;
        }

        if (_downFxInstance?.name == effectName)
        {
            Destroy(_downFxInstance.gameObject);
            _downFxInstance = null;
            return;
        }
    }

    private bool IsIgniteOver()
    {
        return _currentFxTime >= _IgniteTime;
    }

    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }
}
