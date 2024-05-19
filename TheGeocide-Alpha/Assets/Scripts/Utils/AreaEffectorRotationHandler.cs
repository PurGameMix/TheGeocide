using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotation on a gameobject dont change the ForceMagnitude of AreaEffector
/// </summary>
public class AreaEffectorRotationHandler : MonoBehaviour
{
    [SerializeField]
    private Transform _origin;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private bool _isRepulse;
    [SerializeField]
    private AreaEffector2D _effector;

    private float _absMagnitude;
    private bool _isLookingLeft;
    // Start is called before the first frame update
    void Start()
    {
        _absMagnitude = Mathf.Abs(_effector.forceMagnitude);
        _isLookingLeft = _origin.position.x > _target.position.x;
        HandleRotation();
    }
    void FixedUpdate()
    {
        var currentlyLookingLeft = _origin.position.x > _target.position.x;

        if (currentlyLookingLeft != _isLookingLeft)
        {
            _isLookingLeft = currentlyLookingLeft;
            HandleRotation();
        }
    }

    private void HandleRotation()
    {
        if (_isLookingLeft)
        {
            _effector.forceMagnitude = _isRepulse ? -_absMagnitude : _absMagnitude;
            return;
        }

        _effector.forceMagnitude = _isRepulse ? _absMagnitude : -_absMagnitude;
        return;
    }


}
