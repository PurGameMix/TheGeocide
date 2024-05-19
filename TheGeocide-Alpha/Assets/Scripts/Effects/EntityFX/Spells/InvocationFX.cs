using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvocationFX : InitionFX
{
    [SerializeField]
    private Animator _animator;

    internal override void Destroy(Action onCompleted = null)
    {
        _animator.Play("Idle");
        Destroy(gameObject);
    }

    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }

    internal override void EffectCompleted(string effectName)
    {
        throw new NotImplementedException();
    }

    internal override void Init(Action onCompleted = null)
    {
        _animator.Play("Effect");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
