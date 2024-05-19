using System;
using System.Collections;
using UnityEngine;

public class WraithAI : EnemyAI
{
    private EnemyRangedWeapon _enemyRangedWeapon;
    private bool _previousValue= false;


    // Start is called before the first frame update
    private void Start()
    {
        //var fleeParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        _enemyRangedWeapon = gameObject.GetComponent<EnemyRangedWeapon>();
        _stateMachine = new StateMachine();
        //rb = gameObject.GetComponent<Rigidbody2D>();

        var patrol = new Patrol(this);
        var moveToPlayer = new FlyToAttack(this);
        var attack = new MagicAttack(this);
        var flee = new FleeAway(this);
        var hurt = new Hurt(this);
        var die = new Die(this);

        At(patrol, moveToPlayer, IsPlayerInAggroRange);
        At(moveToPlayer, patrol, StuckWhileMoving());
        At(moveToPlayer, attack, IsPlayerInAttackRange);
        At(attack, moveToPlayer, OutOfAttackRange);

        //_stateMachine.AddAnyTransition(flee, LowHealth);
        _stateMachine.AddAnyTransition(hurt, IsAttacked);
        _stateMachine.AddAnyTransition(die, HasNoHealth);

        //At(flee, patrol, () => OutOfRange());
        At(hurt, patrol, () => HurtingEnemyFinished());

        _stateMachine.SetState(patrol);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        Func<bool> StuckWhileMoving() => () => moveToPlayer.TimeStuck > 3f || OutOfAttackRange();
    }

    private bool HurtingEnemyFinished()
    {
        //Debug.Log($"IsAttacked: {IsAttacked()} && IsAnimationPlaying {IsAnimationPlaying()} && IsPLayingSOund {_audioController.IsPlaying("hurt")} ");
        return !IsAttacked();
    }

    private void FixedUpdate()
    {
        var currentValue = IsPlayerInAggroRange();
        if (_previousValue != currentValue)
        {
            _previousValue = currentValue;
            Debug.Log("PlayerDetected: "+ currentValue);
        }
        
        _stateMachine.Tick();
    }

    #region Event processor

        internal override void BasicAttack()
        {
            throw new NotImplementedException();
        }

        internal override void BasicAttackCompleted()
        {
            throw new NotImplementedException();
        }

    internal override bool IsGroundUnit()
    {
        return false;
    }
    internal override void BasicAttackBegin()
    {
        _enemyRangedWeapon.Shoot();
    }

    internal override void BasicAttackSlash()
    {
        throw new NotImplementedException();
    }

    //internal override void HurtCompleted()
    //{
    //    _isAttacked = false;
    //}

    internal override void RepulseAttackBegin()
    {
        throw new NotImplementedException();
    }

    internal override void RepulseAttackSlash()
    {
        throw new NotImplementedException();
    }

    internal override void RepulseAttack()
    {
        throw new NotImplementedException();
    }

    internal override void RepulseAttackCompleted()
    {
        throw new NotImplementedException();
    }

    internal override void SpecialAttackAnimationBegin()
    {
        throw new NotImplementedException();
    }

    internal override void SpecialAttackSlash()
    {
        throw new NotImplementedException();
    }

    internal override void SpecialAttackBegin()
    {
        throw new NotImplementedException();
    }

    internal override void SpecialAttackEnd()
    {
        throw new NotImplementedException();
    }

    internal override void SpecialAttackAnimationCompleted()
    {
        throw new NotImplementedException();
    }

    internal override void AimingLockBegin()
    {
        throw new NotImplementedException();
    }

    internal override void AimingLockEnd()
    {
        throw new NotImplementedException();
    }
    #endregion

}
