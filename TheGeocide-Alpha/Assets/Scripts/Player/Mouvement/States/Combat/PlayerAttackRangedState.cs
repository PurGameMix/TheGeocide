using Assets.Data.PlayerMouvement.Definition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRangedState : PlayerUsingAbilityState
{
	public PlayerAttackRangedState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

    public override void LogicUpdate()
	{
		base.LogicUpdate();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override PlayerStateType GetMouvementType()
	{
		return PlayerStateType.AttackRanged_Cast;
	}

    internal bool CanRelease()
    {
		return true;
        //throw new NotImplementedException();
    }

    internal void Launch()
    {
        //throw new NotImplementedException();
    }
}
