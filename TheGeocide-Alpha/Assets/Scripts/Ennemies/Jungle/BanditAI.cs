using Assets.Data.Enemy.Definition;
using Assets.Data.PlayerMouvement.Definition;
using Assets.Scripts.Ennemies.Weapons;
using System;
using UnityEngine;

public class BanditAI : EnemyAI
{
    public bool DisplayDebugLog;
    [SerializeField]
    private EnemyMeleeWeapon _enemyMelee;
    [SerializeField]
    private Firearm _enemyWeapon;

    private void Awake()
    {
        Health = 100;
        _basicAttackCD = 3f;
        _specialAttackCD = 10f;
        _closeAttackCD = 1f;
        _missHitRecoveryTime = 5f;

        _surpriseStateCD = 60f;
        _lastSurpriseStateTime = -_surpriseStateCD;

        _alarmStateCD = 60f;
        _lastAlarmStateTime = -_alarmStateCD;

        _isIdleLocked = IsIdleLockedOnStart;

        _playerStateChannel.OnMouvementStateEnter += OnPlayerDeath;
        _aiStateChannel.OnStateEnter += OnStateEnter;
        _aiStateChannel.OnStateExit += OnStateExit;
        if (_alarmChannel != null)
        {
            _alarmChannel.OnAlarmTriggered += OnAlarmTriggered;
        }
    }

    private void OnDestroy()
    {
        _aiStateChannel.OnStateEnter -= OnStateEnter;
        _aiStateChannel.OnStateExit -= OnStateExit;
        _playerStateChannel.OnMouvementStateEnter -= OnPlayerDeath;
        if (_alarmChannel != null)
        {
            _alarmChannel.OnAlarmTriggered -= OnAlarmTriggered;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _stateMachine = new StateMachine(_aiStateChannel, name);

        //state
        var idle = new Idle(this);
        var patrol = new Patrol(this);
        var surprised = new Surprised(this);
        var alarm = new Alarm(this);
        var moveToPlayer = new MoveToAttack(this);
        var moveToCheck = new MoveToSuspecious(this);
        var check = new CheckSuspecious(this);
        var idleCombat = new IdleCombat(this);
        var basicAttack = new BasicAttack(this);
        var contactAttack = new CloseAttack(this);
        var specialAttack = new SpecialAttack(this);
        var flee = new FleeAway(this);
        var hurt = new Hurt(this);
        var ignited = new Ignite(this);
        var electrocuted = new Electrocute(this);
        var die = new Die(this);

        //From Idle
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        At(idle, patrol, IsIdleEnough);
        At(idle, moveToPlayer, IsInAggroRangeAndSupriseOnCD);
        At(idle, basicAttack, IsPossibleToBasicAttack);
        At(idle, idleCombat, IsPlayerInAttackRangeButNoCD);
        At(idle, contactAttack, IsPossibleToBasicAttack);

        //From Patrol
        At(patrol, surprised, CanBeSurprised);
        At(patrol, moveToPlayer, IsInAggroRangeAndSupriseOnCD);
        At(patrol, basicAttack, IsPossibleToBasicAttack);
        At(patrol, idleCombat, IsPlayerInAttackRangeButNoCD);
        At(patrol, contactAttack, IsPossibleToBasicAttack);

        //From surprised
        At(surprised, alarm, CanAlarmOther);
        At(surprised, moveToPlayer, CanAggroPlayer);
        At(surprised, moveToCheck, CanMoveToCheckSuspecious);

        //From alarm
        At(alarm, idleCombat, IsAlarmFinished);

        //From move to suspecious
        At(moveToCheck, moveToPlayer, IsPlayerInAggroRange);
        At(moveToCheck, check, SuspeciousPointReached);

        //from checking suspecious
        At(check, moveToPlayer, IsPlayerInAggroRange);
        At(check, patrol, CheckFinished);

        //From moving to player
        At(moveToPlayer, moveToCheck, OutOfRange);
        At(moveToPlayer, patrol, StuckWhileMoving);
        At(moveToPlayer, basicAttack, IsPossibleToBasicAttack);
        At(moveToPlayer, idleCombat, IsPlayerInAttackRangeButNoCD);
        At(moveToPlayer, contactAttack, IsPossibleToBasicAttack);

        //From basicAttack
        At(basicAttack, moveToPlayer, CanReposition);
        At(basicAttack, patrol, PlayerDead);
        At(basicAttack, idleCombat, IsBasicAttackOutOfCD);
        At(basicAttack, contactAttack, IsPossibleToCloseAttack);

        //From contactAttack
        At(contactAttack, moveToPlayer, CanReposition);
        At(contactAttack, patrol, PlayerDead);
        At(contactAttack, specialAttack, IsPossibleToSpecialAttack);
        At(contactAttack, idleCombat, IsCloseAttackOutOfCD);

        //From chargeAttack
        At(specialAttack, moveToPlayer, CanReposition);
        At(specialAttack, idleCombat, SpecialAttackOutOfCD);

        //From IdleCombat
        At(idleCombat, moveToPlayer, IsPlayerInAggroRangeStrict);
        At(idleCombat, moveToCheck, OutOfRange);
        At(idleCombat, patrol, PlayerDead);
        At(idleCombat, basicAttack, IsPossibleToBasicAttack);
        At(idleCombat, contactAttack, IsPossibleToCloseAttack);

        //From hurtState
        At(hurt, idleCombat, () => !IsAttacked());
        At(ignited, idleCombat, () => !IsIgnited());
        At(electrocuted, idleCombat, () => !IsElectrocuted());
        At(hurt, moveToCheck, () => OutOfRange());

        //From Any states
        _stateMachine.AddAnyTransition(die, HasNoHealth);
        _stateMachine.AddAnyTransition(hurt, CanBeHurt);
        _stateMachine.AddAnyTransition(ignited, IsIgnited);
        _stateMachine.AddAnyTransition(electrocuted, IsElectrocuted);
        _stateMachine.AddAnyTransition(idle, CanDefaultState);
        _stateMachine.AddAnyTransition(moveToCheck, CanAlarmCheck);
        //_stateMachine.AddAnyTransition(flee, LowHealth);

        //At(flee, patrol, () => OutOfAggroRange());
        _stateMachine.SetState(idle);

    }

    private void FixedUpdate()
    {
        _stateMachine.Tick(DisplayDebugLog);

        //Calculating timers
        if (_isOnState)
        {
            TimeOnState += Time.deltaTime;

            if (_stateMachine.GetCurrentSate().GetStateType().IsMovingState() && _mouvementController.IsAgentStuck(_lastPosition))
            {
                CurrentTimeStuck += Time.deltaTime;
            }
            else
            {
                CurrentTimeStuck = 0;
            }

            _lastPosition = _mouvementController.GetAgentPosition();
        }

        _lastBasicAttackTime -= Time.deltaTime;
        _lastCloseAttackTime -= Time.deltaTime;
        _lastSpecialAttackTime -= Time.deltaTime;

        _lastSurpriseStateTime -= Time.deltaTime;
        _lastAlarmStateTime -= Time.deltaTime;

        HandleFrozenTick();
        HandleElectrocuteTick();
    }

    #region Conditions
    internal override bool IsGroundUnit()
    {
        return true;
    }
    #endregion //Conditions

    #region Event processor
    private void OnPlayerDeath(PlayerStateEvent mvtEvt)
    {
        if (mvtEvt.Type != PlayerStateType.Dead)
        {
            return;
        }

        IsPlayerDead = true;
    }

    private void OnStateExit(EnemyStateEvent stateEvt)
    {
        if (name != stateEvt.EnemyId)
        {
            return;
        }

        _isOnState = false;

        //Debug.Log($"ExitState {stateEvt.Type} with TimeOnState :{TimeOnState}");
        CurrentTimeStuck = 0;
        TimeOnState = 0;      
    }

    private void OnStateEnter(EnemyStateEvent stateEvt)
    {
        if(name != stateEvt.EnemyId)
        {
            return;
        }
        //Debug.Log($"Entering {stateEvt.Type} with TimeOnState :{TimeOnState} and {TimeStuck}");
        _isOnState = true;
    }


    internal override void AimingLockBegin()
    {
        _enemyWeapon.SetAimingLock(true);
    }

    internal override void AimingLockEnd()
    {
        _enemyWeapon.SetAimingLock(false);
    }

    internal override void BasicAttackBegin()
    {
        _isAttacking = true;
        _enemyWeapon.SetAiming(_threatDetection.GetFocusTarget(ThreatDetectionType.Attack));
    }


    internal override void BasicAttack()
    {
        _enemyWeapon.Shoot();
        _lastBasicAttackTime =  0;
    }
    internal override void BasicAttackSlash()
    {
        _effectsController.Play("slash", _mouvementController.IsAgentLookingLeft());
        PlaySwingSound();
    }

    internal override void BasicAttackCompleted()
    {
        //Debug.Log($"{DateTime.UtcNow}: MeleeAttackCompleted");
        _isAttacking = false;
        _enemyWeapon.SetAiming(null);
    }

    internal override void RepulseAttackBegin()
    {
        _isAttacking = true;
    }

    internal override void RepulseAttackSlash()
    {
        PlaySwingSound();
    }

    internal override void RepulseAttack()
    {
        var hasHit = _enemyMelee.RepulseAttack();
        _lastCloseAttackTime = hasHit ? 0 : _missHitRecoveryTime;
        if (hasHit)
        {
            PlayKickSound();
        }
    }

    internal override void RepulseAttackCompleted()
    {
        //Debug.Log($"{DateTime.UtcNow}: RepulseAttackCompleted");
        _isAttacking = false;
    }

    internal override void SpecialAttackAnimationBegin()
    {
        _effectsController.Play("charge_cast", _mouvementController.IsAgentLookingLeft());
        _audioController.Play("charging");
        _isAttacking = true;
        
    }

    internal override void SpecialAttackSlash()
    {
        _effectsController.Play("charge_release", _mouvementController.IsAgentLookingLeft());
        PlaySwingSound();
    }

    internal override void SpecialAttackBegin()
    {
        _enemyWeapon.Shoot();
        _lastSpecialAttackTime =  0;
    }

    internal override void SpecialAttackEnd()
    {
        throw new NotImplementedException();
    }

    internal override void SpecialAttackAnimationCompleted()
    {
        //Debug.Log($"{DateTime.UtcNow}: RepulseAttackCompleted");
        _isAttacking = false;
    }
    #endregion //Event processor

    private void PlaySwingSound()
    {
        var rng = UnityEngine.Random.Range(1, 3);
        //Debug.Log("swing_" + rng);
        _audioController.Play("swing_" + rng);
    }

    private void PlayHitSound(bool isHeavy)
    {
        if (isHeavy)
        {
            _audioController.Play("hit_heavy");
            return;
        }

        _audioController.Play("hit_light");
    }

    private void PlayKickSound()
    {
        _audioController.Play("hit_kick");
    }
}
