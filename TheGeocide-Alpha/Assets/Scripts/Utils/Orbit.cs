using System;
using UnityEngine;

public class Orbit : MonoBehaviour
{

    public float SpreadX;
    public float SpreadY;
    public Transform CenterPoint;
    public float RotationSpeed;
    public float StartRadAngle;
    private bool IsClockwiseRotation;

    private float _timer;

    //Putting into orbit
    public float InitDelay;
    private float _xDelta;
    private float _yDelta;
    private float _currentXSpread;
    private float _currentYSpread;
    private float orbitTolerence = 0.01f;

    private bool _start;
    private Action _onOrbitReached;
    private void Start()
    {
        //Start(null);
    }

    internal void Start(Action onOrbitReached)
    {
        _timer = StartRadAngle;

        if (InitDelay > 0)
        {
            _xDelta = SpreadX / InitDelay;
            _yDelta = SpreadY / InitDelay;
        }
        else
        {
            _xDelta = 1;
            _yDelta = 1;
            _currentXSpread = SpreadX;
            _currentYSpread = SpreadY;
        }

        _onOrbitReached = onOrbitReached;
        _start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_start)
        {
            return;
        }

        _timer += Time.deltaTime * RotationSpeed;

        if(_currentXSpread < SpreadX)
        {
            _currentXSpread += _xDelta * Time.deltaTime;
        }

        if (_currentXSpread > SpreadX)
        {
            _currentXSpread -= _xDelta * Time.deltaTime;
        }

        if (_currentYSpread < SpreadY)
        {
            _currentYSpread += _yDelta * Time.deltaTime;
        }

        if (_currentYSpread > SpreadY)
        {
            _currentYSpread -= _yDelta * Time.deltaTime;
        }

        if (SpreadX - orbitTolerence < _currentXSpread && _currentXSpread < SpreadX + orbitTolerence
            && SpreadY - orbitTolerence < _currentYSpread && _currentYSpread < SpreadY + orbitTolerence 
            && _onOrbitReached != null)
        {
            _onOrbitReached();
            _onOrbitReached = null;
        }

        Rotate();
    }

    /// <summary>
    /// Usefull to change orbite live
    /// </summary>
    /// <example>
    /// Want a object orbiting slowing crash in spirals
    /// </example>
    /// <param name="x"></param>
    /// <param name="y"></param>
    internal void UpdateOrbiting(float x, float y, float speed)
    {
        SpreadX = x;
        SpreadY = y;
        RotationSpeed = speed;
    }

    void Rotate()
    {
        float clock = IsClockwiseRotation ? -1 : 1;

        float x = Mathf.Cos(_timer) * _currentXSpread * clock;
        float y = Mathf.Sin(_timer) * _currentYSpread;

        Vector3 pos = new Vector3(x, y, 0);
        transform.position = pos + CenterPoint.position;
    }

    private void OnDrawGizmos()
    {

    //#if UNITY_EDITOR
        //UnityEditor.Handles.color = Color.black;
        //var reposition = new Vector3(-0.1f, 0.1f, 0);

        //var degAngle = _timer * 180 /Mathf.PI;
        //UnityEditor.Handles.Label(transform.position + reposition, "Orbit " + degAngle);
    //#endif

    }
}
