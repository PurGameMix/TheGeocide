using System;
using UnityEngine;

public class FrostAimAssist : AimAssist
{

    public float AimTime;
    [SerializeField]
    private Transform _topPoint;
    private Vector2 _topPosition;
    [SerializeField]
    private Transform _bottomPoint;
    private Vector2 _botPosition;
    [SerializeField]
    private AudioController _audioController;

    private float _aimTick;
    private int _aimAngle = 90;

    private float _lastTick;
    private int _degreeCount = 0;
    private bool _firstEntryHold = true;
    private bool _firstEntryCast = true;
    private bool _alternator;
    private bool _playing = true;

    // Start is called before the first frame update
    void Start()
    {
        _aimTick = AimTime / _aimAngle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        _lastTick += Time.deltaTime;

        if (_degreeCount >= _aimAngle)
        {
            //Debug.Log($"aimTick: {_aimTick},_test: {_test}");
            _topPosition = _topPoint.position;
            _botPosition = _bottomPoint.position;
            HandlePressureAnimation();
            PlayHoldingSound();
            return;
        }

        
        if(_lastTick >= _aimTick && _playing)
        {
            PlayCastingSound();
            _topPoint.Rotate(new Vector3(0,0,-1));
            _bottomPoint.Rotate(new Vector3(0, 0, 1));
            _lastTick = 0;
            _degreeCount++;
        }
    }

    private void HandlePressureAnimation()
    {
        if (_lastTick >= _aimTick * 3)
        {
            
            var rngX = UnityEngine.Random.Range(0, 3) / 1000f;
            var rngY = UnityEngine.Random.Range(0, 1) / 1000f;
            //Debug.Log($"rngX: {rngX},rngY: {rngY}");

            _alternator = !_alternator;
            if (_alternator)
            {
               
                _topPoint.position = new Vector2(_topPosition.x + rngX, _topPosition.y + rngY);
                _topPoint.position = new Vector2(_topPosition.x + rngX, _topPosition.y + rngY);

                _bottomPoint.position = new Vector2(_botPosition.x - rngX, _botPosition.y - rngY);
                _bottomPoint.position = new Vector2(_botPosition.x - rngX, _botPosition.y - rngY);
            }
            else
            {
                _topPoint.position = new Vector2(_topPosition.x - rngX, _topPosition.y - rngY);
                _topPoint.position = new Vector2(_topPosition.x - rngX, _topPosition.y - rngY);

                _bottomPoint.position = new Vector2(_botPosition.x + rngX, _botPosition.y + rngY);
                _bottomPoint.position = new Vector2(_botPosition.x + rngX, _botPosition.y + rngY);
            }

            _lastTick = 0;
        }
            
    }

    private void PlayHoldingSound()
    {
        if (_firstEntryHold)
        {
            _firstEntryHold = false;
            _audioController.Stop("Casting");
            _audioController.Play("Holding");
        }
    }

    private void PlayCastingSound()
    {
        if (_firstEntryCast)
        {
            _firstEntryCast = false;
            _audioController.Play("Casting");
        }
       
    }

    public override float GetAimWindow()
    {
        return 180 - (_degreeCount * 2);
    }

    public override void Stop()
    {
        _playing = false;
        _audioController.Stop("Casting");
    }

    public override void SetCursor(Transform cursorTransform)
    {
        //no need
    }

    public override Transform GetPosition()
    {
        throw new NotImplementedException();
    }

    public override void SetCastTime(float castTime)
    {
        AimTime = castTime;
    }
}
