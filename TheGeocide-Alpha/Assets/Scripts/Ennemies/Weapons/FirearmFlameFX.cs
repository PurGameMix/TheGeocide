using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmFlameFX : InitionFX
{
    [SerializeField]
    private List<SpriteRenderer> flamesDisplay;
    private SpriteRenderer _currentDisplaySr;

    private float _flameDisplayTime = 0.1f;
    private float _currentFlameTime = 0;
    private int _currentFlameIndex = 0;
    private bool _isFlameRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        if (flamesDisplay == null || flamesDisplay.Count == 0)
        {
            Debug.LogWarning("No flames to display");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isFlameRunning)
        {
            return;
        }

        if(_currentFlameTime >= _flameDisplayTime)
        {
            _isFlameRunning = false;
            _currentDisplaySr.enabled = false;
            HandleListIndex();      
        }
    }

    private void HandleListIndex()
    {
        _currentFlameIndex++;
        if (_currentFlameIndex == flamesDisplay.Count)
        {
            _currentFlameIndex = 0;
        }
    }

    internal override void Destroy(Action onCompleted = null)
    {
        throw new NotImplementedException();
    }

    internal override void EffectCompleted(string effectName)
    {
        throw new NotImplementedException();
    }

    internal override void Init(Action onCompleted = null)
    {

        if (_isFlameRunning)
        {
            _currentDisplaySr.enabled = false;
            HandleListIndex();
        }

        _flameDisplayTime = 0;
        _isFlameRunning = true;
        _currentDisplaySr = flamesDisplay[_currentFlameIndex];
        _currentDisplaySr.enabled = true;
    }

    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }
}
