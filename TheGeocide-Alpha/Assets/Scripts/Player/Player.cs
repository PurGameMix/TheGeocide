using System;
using System.Collections;
using UnityEngine;

public class Player : CanBeDetect, ICanBeDamaged , ICanBeRepulsed
{

    public float DamageRecover = 1f;
    public float CCRecover = 0.5f;

    [SerializeField]
    private AudioController _playerAudioController;

    [SerializeField]
    private Transform _centerPoint;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private GameStateChannel _gameStateChannel;

    [SerializeField]
    private PlayerStateChannel _playerStateChannel;


    private Coroutine _currentDot;

    public float _lastDamageTimer { get; private set; }
    public float _lastCCTimer { get; private set; }
    private void Awake()
    {
        _currentDot = null;
        _playerStateChannel.OnTakeDamageRequest += OnTakeDamageRequest;
    }

    private void OnDestroy()
    {
        _playerStateChannel.OnTakeDamageRequest -= OnTakeDamageRequest;
    }

    internal void Drown(bool v)
    {
        if (v)
        {
            _currentDot = StartCoroutine(DamageOverTime(20, 1));
        }
        else
        {
            //Debug.Log("_stopChanged!");
            StopCoroutine(_currentDot);
        }
        
    }

    //todo: Had nbTick instead of decrease function?
    private IEnumerator DamageOverTime(int dmg, int seconds)
    {
        while (dmg > 0)
        {
            yield return new WaitForSeconds(seconds);
            _playerAudioController.Play("hurt_water");
            TakeDamage(dmg, HealthEffectorType.dot);
            dmg = dmg - (dmg / 2 + 1); //decrease function
        }
    }

    internal Transform GetCenterPoint()
    {
        return _centerPoint;
    }

    private void Update()
    {
        _lastDamageTimer -= Time.deltaTime;
        _lastCCTimer -= Time.deltaTime;
    }

    private void OnTakeDamageRequest(PlayerTakeDamageEvent tdEvt)
    {
        TakeDamage(tdEvt.Damage, tdEvt.Origin);
    }

    public void TakeDamage(int damage, HealthEffectorType type)
    {
        if(_lastDamageTimer >= 0)
        {
            return;
        }

        _gameStateChannel.RaiseHealthChanged(new HealthEvent() { 
            Difference = -damage,
            Origin = type
        });

        _lastDamageTimer = DamageRecover;
    }

    public void TakeRepulse(Vector2 repulseMagnitude)
    {
        //todo move on StateMachine with animation?   
        _rb.AddForce(repulseMagnitude);
    }

    public void TakeKnockBack(Vector2 repulseMagnitude, float stunDuration = 1.5f)
    {
        if (_lastCCTimer >= 0)
        {
            return;
        }
        _playerStateChannel.RaiseStateEffector(new StateEffectorEvent()
        {
            Duration = stunDuration,
            Direction = repulseMagnitude,
            Type = StateEffectorType.stun
        });

        _lastCCTimer = CCRecover;
    }

    //Event from Animator trigger

    #region Event trigger
    public void DeathCompleted()
    {
        //Destroy(gameObject);
        _playerStateChannel.RaiseOnDeath();
    }

    public void LeftStepCompleted()
    {
        _playerAudioController.Play("StepLeftGrass");
    }

    public void RightStepCompleted()
    {
        _playerAudioController.Play("StepRightGrass");
    }

    public void SwimLightCompleted()
    {
        _playerAudioController.Play("SwimLight");
    }

    public void SwimDiveCompleted()
    {
        _playerAudioController.Play("SwimDive");
    }

    public void SwimHeavyCompleted()
    {
        _playerAudioController.Play("SwimHeavy");
    }

    
    #endregion
}
