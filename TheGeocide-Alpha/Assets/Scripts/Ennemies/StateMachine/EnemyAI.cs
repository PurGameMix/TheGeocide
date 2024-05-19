using Assets.Data.Enemy;
using Assets.Data.Enemy.Definition;
using Assets.Data.GameEvent.Definition;
using Assets.Scripts.PathBerserker2dExtentions;
using System.Collections;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour, ICanBeDamaged, ICanBeRepulsed
{

    internal StateMachine _stateMachine;

    [SerializeField]
    internal EnemyIAMouvement _mouvementController;
    [SerializeField]
    internal EnemyThreatController _threatDetection;
    [SerializeField]
    internal EnemyStateChannel _aiStateChannel;
    [SerializeField]
    internal PlayerStateChannel _playerStateChannel;
    [SerializeField]
    private GameEventChannel _geChannel;
    [SerializeField]
    internal EnemyAlarmChannel _alarmChannel;

    [SerializeField]
    internal AudioController _audioController;
    [SerializeField]
    internal EffectsController _effectsController;

    [SerializeField]
    internal bool _autoCleanDeath;
    [SerializeField]
    internal SoulSpawner _soulSpawner;
    internal int _soulDropPercent = 33;
    internal int _minSoulAmount = 5;
    internal int _maxSoulAmount = 20;

    //internal Rigidbody2D rb;

    //Timers
    internal bool _isOnState = false;
    internal Vector2 _lastPosition = Vector2.zero;
    internal float CurrentTimeStuck;
    internal float TimeStuckOnStateCD = 3f;
    internal float TimeStuckOnMovingCD = 1f; 
    internal float TimeOnIdleState = 3f;
    public bool IsIdleLockedOnStart;
    internal bool _isIdleLocked;
    public bool IsNoSurpriseOnStart;
    internal float TimeOnState;

    //CD
    internal float _lastBasicAttackTime;
    internal float _lastCloseAttackTime;
    internal float _lastSpecialAttackTime;
    internal float _basicAttackCD;
    internal float _specialAttackCD;
    internal float _closeAttackCD;
    internal float _missHitRecoveryTime;
    internal float _lastSurpriseStateTime;
    internal float _surpriseStateCD;

    internal float _lastAlarmStateTime;
    internal float _alarmStateCD;

    //Locked states
    internal bool _isAttacked;
    internal bool _isAttacking;
    internal bool _isInterruptibleAttack;
    internal bool IsPlayerDead;
    internal bool _isDead;
    internal bool _isSurpriseCompleted;
    internal bool _isAlarmCompleted;
    internal bool _isCheckCompleted;
    internal bool _isAlarmTriggeredByCrew;
    //ignite
    internal float _igniteDamage;

    //frozen
    internal int _frozenTick = 0;
    internal int _frozenMax = 10;
    internal float _frozenPerditionCd = 5f;
    internal float _lastfrozenTick;
    internal float _frozenUnfreezeCd = 0.5f;
    internal float _lastUnfreeze;

    internal int _frozenCritMultiplier = 2;

    //Electrocute
    internal int _elecTick = 0;
    internal int _elecMax = 3;
    internal bool _isElectrocuted = false;

    internal float Health = 100;

    #region Animator events 
    internal abstract void SpecialAttackAnimationBegin();
    internal abstract void SpecialAttackSlash();
    internal abstract void SpecialAttackBegin();
    internal abstract void SpecialAttackEnd();
    internal abstract void SpecialAttackAnimationCompleted();

    internal abstract void RepulseAttackBegin();
    internal abstract void RepulseAttackSlash();
    internal abstract void RepulseAttack();
    internal abstract void RepulseAttackCompleted();

    internal abstract void BasicAttackBegin();
    internal abstract void BasicAttackSlash();
    internal abstract void BasicAttack();
    internal abstract void BasicAttackCompleted();
    internal abstract void AimingLockBegin();
    internal abstract void AimingLockEnd();
    internal void HurtCompleted()
    {
        Debug.Log("HurtCompleted IsAttacked_0: "+ _isAttacked);
        _isAttacked = false;
        Debug.Log("HurtCompleted IsAttacked_1: " + _isAttacked);
    }

    internal void SurpriseCompleted(){
        _isSurpriseCompleted = true;
    }

    internal void AlarmCompleted()
    {
        _isAlarmCompleted = true;
    }

    internal void BroadcastAlarm()
    {
        _alarmChannel.RaiseAlarm(new EnemyAlarmEvent(_threatDetection.GetFocusTarget(ThreatDetectionType.Aggressive)));
    }

    internal void OnAlarmTriggered(EnemyAlarmEvent alarmEvent)
    {
        _isAlarmTriggeredByCrew = true;
    }

    internal void ElecStunCompleted(){
        _audioController.Stop("electrocute");
        _isElectrocuted = false;
    }
    internal void CheckCompleted(){
        _isCheckCompleted = true;
    }
    internal void DeathCompleted(){
        _isDead = true;

        RegisterGameEvent();
        CleanEffects();
        SpawnSoul();
        if (_autoCleanDeath)
        {
            StartCoroutine(CleanAfterDeath(3));
        }
    }

    private void CleanEffects()
    {
        

        _igniteDamage = 0;

        if(_frozenTick > 0)
        {
            _frozenTick = 0;
            _aiStateChannel.RaiseOnFrozenTickChange(new FrozenStateEvent()
            {
                EnemyId = name,
                FrozenMax = _frozenMax,
                FrozenTick = _frozenTick
            });
        }

        if(_elecTick > 0)
        {
            _elecTick = 0;
            _aiStateChannel.RaiseOnElectrocuteTickChange(new ElectrocuteStateEvent()
            {
                EnemyId = name,
                ElecMax = _elecMax,
                ElecTick = _elecTick
            });
        }

    }

    private void RegisterGameEvent()
    {
        if (_geChannel == null)
        {
            Debug.LogWarning($"No game event channel registered");
            return;
        }
        _geChannel.RaiseEvent(new GameEvent() { 
            Origin = gameObject,
            Type = GameEventType.EnemyDie
        });
    }

    internal void LeftStepCompleted(){
        _audioController.Play("StepLeft");
    }
    internal void RightStepCompleted(){
        _audioController.Play("StepRight");
    }

    #endregion //Animator events

    #region Timeline events

    #endregion //Timeline events

    private void SpawnSoul()
    {
        var rng = Random.Range(0, 100);
        if(rng <= _soulDropPercent)
        {
            rng = Random.Range(_minSoulAmount, _maxSoulAmount);
            _soulSpawner.Spawn(rng);
        }
    }

    private IEnumerator CleanAfterDeath(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    #region State global conditions
    internal abstract bool IsGroundUnit();

    internal bool IsAttacked()
    {
        return _isAttacked && !IsIgnited() && !IsElectrocuted() && IsValidatingCommon();
    }

    internal bool CanBeHurt()
    {
        return IsAttacked() && !CurrentStateIs(EnemyStateType.Hurt) && IsValidatingCommon();
    }

    internal bool CurrentStateIs(EnemyStateType testState)
    {
        return _stateMachine.GetCurrentSate().GetStateType() == testState;
    }

    internal bool IsIgnited()
    {
        return _igniteDamage > 0 && !HasNoHealth() && IsValidatingCommon();
    }

    internal bool IsElectrocuted()
    {
        return _isElectrocuted && IsValidatingCommon();
    }

    internal bool LowHealth()
    {
        return Health > 0 && Health < 33;
    }

    internal bool HasNoHealth()
    {
        return Health <= 0;
    }

    internal bool CheckFinished()
    {
        return _isCheckCompleted;
    }
    internal bool IsDetectionStatesFinished()
    {
        return _isSurpriseCompleted && _isAlarmCompleted;
    }
    
    internal bool IsSurpriseFinished()
    {
        return _isSurpriseCompleted;
    }

    internal bool IsAlarmFinished()
    {
        return _isAlarmCompleted;
    }

    internal bool IsSurpriseCoolDownReady()
    {
        return _lastSurpriseStateTime <= -_surpriseStateCD;
    }

    internal bool IsAlarmCoolDownReady()
    {
        return _alarmChannel!= null && _lastAlarmStateTime <= -_surpriseStateCD;
    }

    internal bool IsEnemyAbleToAttackPlayer()
    {
        return IsPossibleToAttack() && _mouvementController.IsGrounded() && IsValidatingCommon();
    }

    private bool IsPossibleToAttack()
    {
        return !_isAttacking || _isInterruptibleAttack;
    }

    internal bool PlayerDead()
    {
        return IsPlayerDead;
    }
    internal bool StuckWhileMoving()
    {
        return CurrentTimeStuck > TimeStuckOnMovingCD;
    }

    internal bool IsIdleEnough()
    {

        return TimeOnState > TimeOnIdleState && IsValidatingCommon();
    }

    private bool IsIAStuck()
    {
        return CurrentTimeStuck > TimeStuckOnStateCD;
    }

    internal bool CanDefaultState()
    {
        return !CurrentStateIs(EnemyStateType.Idle)  && (IsIAStuck() && !HasNoHealth() || _isIdleLocked) ;
    }

    internal bool CanAlarmCheck()
    {
        return _isAlarmTriggeredByCrew && IsValidatingCommon();
    }

    internal bool IsValidatingCommon()
    {
        return !_isIdleLocked && !HasNoHealth();
    }

    public void LockStateMachine()
    {
        _isIdleLocked = true;
    }

    public void UnlockStateMachine()
    {
        //Ignored the first time
        if (IsIdleLockedOnStart)
        {
            IsIdleLockedOnStart = false;
            return;
        }
        _isIdleLocked = false;
    }

    internal bool SuspeciousPointReached()
    {
        return CurrentTimeStuck > 0.2f; //Todo use goal reached mvtController
    }
    #endregion //State global conditions

    #region State Range Conditions
    internal bool IsPlayerInAggroRangeStrict()
    {
        //Debug.Log("IsInAggroRange: " + _threatDetection.PlayerInAggroRange());
        return _threatDetection.PlayerInStrictAggroRange() && IsValidatingCommon();
    }

    internal bool IsPlayerInAggroRange()
    {
        //Debug.Log("IsInAggroRange: " + _threatDetection.PlayerInAggroRange());
        return _threatDetection.PlayerInAggroRange() && IsValidatingCommon();
    }

    internal bool IsEnemyLookInPlayerDirection()
    {
        var isLookingLeft = _mouvementController.IsAgentLookingLeft();
        var diff = _threatDetection.GetPlayerTransform().position.x - transform.position.x;
        return isLookingLeft && diff < 0 || !isLookingLeft && diff > 0;
    }

    internal bool IsPlayerInAttackRange()
    {
        return _threatDetection.EntityInAttackRange() && IsValidatingCommon();
    }

    internal bool IsPlayerAheadAttackRange()
    {
        return _threatDetection.PlayerAheadAttackRange() && IsValidatingCommon();
    }


    internal bool IsInCloseRange()
    {
        //Debug.Log("PlayerInAttackRange: " + _threatDetection.PlayerInAggroRange());
        return _threatDetection.EntityInCloseRange() && !IsPlayerDead;
    }

    internal bool OutOfRange()
    {
        return OutOfAggroRange() && OutOfAttackRange();
    }

    internal bool OutOfAggroRange()
    {
        return !IsPlayerInAggroRange();
    }

    internal bool OutOfAttackRange()
    {
        return !IsPlayerInAttackRange();
    }

    internal bool CanBeSurprised()
    {
        var canGoToState = IsPlayerInAggroRange()&& IsEnemyLookInPlayerDirection() && IsSurpriseCoolDownReady() && IsValidatingCommon();
        if (IsNoSurpriseOnStart && canGoToState)
        {
            IsNoSurpriseOnStart = false;
            _lastSurpriseStateTime = 0;
            return false;
        }

        return canGoToState;
    }

    internal bool CanAlarmOther()
    {
        var canGoToState = IsPlayerInAggroRange() && IsAlarmCoolDownReady() && IsSurpriseFinished() && IsValidatingCommon();

        return canGoToState;
    }

    internal bool IsInAggroRangeAndSupriseOnCD()
    {
        return IsPlayerInAggroRange() && !IsSurpriseCoolDownReady() && IsValidatingCommon();
    }


    internal bool CanAggroPlayer()
    {
        return IsPlayerInAggroRange() && IsDetectionStatesFinished() && IsValidatingCommon();
    }

    internal bool CanMoveToCheckSuspecious()
    {
        return OutOfRange() && IsDetectionStatesFinished();
    }
    #endregion //State Range Condition

    #region State Attack conditions

    internal bool CanReposition()
    {
        return OutOfAttackRange() && IsEnemyAbleToAttackPlayer();
    }
    internal bool IsBasicAttackReady()
    {
        return _lastBasicAttackTime <= -_basicAttackCD;
    }

    internal bool IsSpecialAttackReady()
    {
        return _lastSpecialAttackTime <= -_specialAttackCD;
    }

    internal bool IsCloseAttackReady()
    {
        return _lastCloseAttackTime <= -_closeAttackCD;
    }

    internal bool IsPlayerInAttackRangeButNoCD()
    {
        return IsPlayerInAttackRange() && IsBasicAttackOutOfCD() && SpecialAttackOutOfCD();
    }

    internal bool IsBasicAttackOutOfCD()
    {
        return !IsBasicAttackReady() && !_isAttacking && !IsPlayerDead;
    }

    internal bool SpecialAttackOutOfCD()
    {
        return !IsSpecialAttackReady() && !_isAttacking && !IsPlayerDead;
    }

    internal bool IsCloseAttackOutOfCD()
    {
        return !IsCloseAttackReady() && !_isAttacking && !IsPlayerDead;
    }

    internal bool IsPossibleToBasicAttack()
    {
        return IsPlayerInAttackRange() && IsBasicAttackReady() && IsEnemyAbleToAttackPlayer();
    }

    internal bool IsPossibleToCloseAttack()
    {
        //Debug.Log("PlayerInAttackRange: " + _threatDetection.PlayerInAggroRange());
        return IsInCloseRange() && IsCloseAttackReady() && IsEnemyAbleToAttackPlayer();
    }

    internal bool IsPossibleToSpecialAttack()
    {
        //Debug.Log($"icicicici : {IsPlayerAheadAttackRange()};{IsSpecialAttackReady()}");
        return IsPlayerAheadAttackRange() && IsSpecialAttackReady() && IsEnemyAbleToAttackPlayer();
    }
    #endregion //State Attack conditions

    public void TakeDamage(int damage, HealthEffectorType type)
    {
        if (_isDead)
        {
            return;
        }

        var isCrit = false;
        int appliedDamages = damage;
        if (_frozenTick == _frozenMax)
        {
            isCrit = true;
            appliedDamages = damage * _frozenCritMultiplier;
        }
        Health -= appliedDamages;

        HandleElementalEffector(damage, type);

        _aiStateChannel.RaiseOnDamageTaken(new EnemyDamageTakenEvent()
        {
            position = transform.position,
            Damage = appliedDamages,
            Type = type,
            IsCriticalStrike = isCrit
        });
    }

    private void HandleElementalEffector(int damage, HealthEffectorType type)
    {
        if (type == HealthEffectorType.ignite)
        {
            _lastfrozenTick = _frozenPerditionCd + 1;
            return;
        }

        if (type == HealthEffectorType.playerIgnite)
        {
            _igniteDamage = damage / 2;
            _lastfrozenTick = _frozenPerditionCd +1;
            return;
        }

        if (type == HealthEffectorType.playerFroze)
        {
            if(_igniteDamage > 0)
            {
                _igniteDamage = 0; //todo add extinguish sound?
            }
           
            if(_frozenTick < _frozenMax)
            {
                _audioController.Play("iceBasic");
                _frozenTick++;
                _aiStateChannel.RaiseOnFrozenTickChange(new FrozenStateEvent()
                {
                    EnemyId = name,
                    FrozenMax = _frozenMax,
                    FrozenTick = _frozenTick
                });
            }
            else
            {
                _audioController.Play("iceCritical");
            }
                      
            _lastfrozenTick = 0;
            return;
        }

        if(type == HealthEffectorType.playerElec)
        {
            if (!_isElectrocuted)
            {
                if (_elecTick < _elecMax)
                {
                    _audioController.Play("elecHit");
                    _elecTick++;
                    _aiStateChannel.RaiseOnElectrocuteTickChange(new ElectrocuteStateEvent()
                    {
                        EnemyId = name,
                        ElecMax = _elecMax,
                        ElecTick = _elecTick
                    });
                }
            }
            else
            {
                return;
            }
            
        }
        //todo: hurt for 1 sec for each weapon? seem OP for channeling weapons
        if (!CurrentStateIs(EnemyStateType.Hurt))
        {
            Debug.Log("New dmg_0: " + _isAttacked);
            _isAttacked = true;
            Debug.Log("New dmg_1: " + _isAttacked);
        }
    }

    public void TakeRepulse(Vector2 repulseMagnitude)
    {
        _mouvementController.TakeRepulse(repulseMagnitude);
    }

    public void TakeKnockBack(Vector2 repulseMagnitude, float stunDuration = 1.5f)
    {
        _mouvementController.TakeRepulse(repulseMagnitude);
    }

    private void FixedUpdate()
    {

        HandleFrozenTick();
    }

    internal void HandleFrozenTick()
    {
        _lastfrozenTick += Time.deltaTime;

        if(_frozenTick == 0)
        {
            return;
        }

        if (_lastfrozenTick < _frozenPerditionCd)
        {
            return;
        }

        _lastUnfreeze += Time.deltaTime;
        if (_lastUnfreeze > _frozenUnfreezeCd)
        {
            _lastUnfreeze = 0;
            _frozenTick--;
            _aiStateChannel.RaiseOnFrozenTickChange(new FrozenStateEvent()
            {
                EnemyId = name,
                FrozenMax = _frozenMax,
                FrozenTick = _frozenTick
            });
        }
    }

    internal void HandleElectrocuteTick()
    {

        if(_elecTick < _elecMax || _isElectrocuted)
        {
            return;
        }

        _isElectrocuted = true;
        _elecTick = 0;

        _aiStateChannel.RaiseOnElectrocuteTickChange(new ElectrocuteStateEvent()
        {
            EnemyId = name,
            ElecMax = _elecMax,
            ElecTick = _elecTick
        });
    }
}
