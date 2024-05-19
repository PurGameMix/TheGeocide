using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerOnClimbState : PlayerState
{
	public PlayerOnClimbState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();
		player.DashState.ResetDashes(); //refresh Dashes upon touching the ground
	}

	public override void Exit()
	{
		base.Exit();
	}

    public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (player.LastPressedJumpTime > 0)
		{
			player.StateMachine.ChangeState(player.WallJumpState);
		}
		else if (player.LastPressedDashTime > 0 && player.DashState.CanDash())
		{
			player.StateMachine.ChangeState(player.DashState);
		}
		else if (player.isOnEdge)
		{
			player.StateMachine.ChangeState(player.EdgeClimbState);
		}
		else if (player.LastOnClimbTime < 0)
		{
			//Debug.Log("ClimbingState: problem");
			stateMachine.ChangeState(player.InAirState);
		}
		else if (player._currentHealth <= 0 && player.LastFallImpactTime < 0)
		{
			player.StateMachine.ChangeState(player.DeadState);
		}
		else if (player.LastWoundedTime > 0 && player.LastFallImpactTime < 0)
		{
			player.StateMachine.ChangeState(player.HurtedState);
		}
	}
}
