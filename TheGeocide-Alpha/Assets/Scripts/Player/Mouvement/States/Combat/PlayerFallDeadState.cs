using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallDeadState : PlayerWoundedState
{
	public PlayerFallDeadState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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

		player.Stun();
	}

	public override PlayerStateType GetMouvementType()
	{
		return PlayerStateType.FallDead;
	}
}
