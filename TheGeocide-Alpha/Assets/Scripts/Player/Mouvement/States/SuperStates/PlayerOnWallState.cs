using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerOnWallState : PlayerState
{
	public PlayerOnWallState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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
		if (player.LastPressedJumpTime > 0 && player.LastOnWallTime > 0 && player.CanWallJump())
		{
			player.StateMachine.ChangeState(player.WallJumpState);
		}
		else if (player.isOnEdge)
		{
			player.StateMachine.ChangeState(player.EdgeClimbState);
		}
		else if (player.LastPressedDashTime > 0 && player.DashState.CanDash())
		{
			player.StateMachine.ChangeState(player.DashState);
		}
		else if (player.LastLandingAttackTime > 0) 
		{
			player.StateMachine.ChangeState(player.LandingState);
		}
		else if (player.LastUppercutAttackTime > 0)
		{
			player.StateMachine.ChangeState(player.UppercutState);
		}
		else if (player.LastOnGroundTime > 0)
		{
			player.StateMachine.ChangeState(player.IdleState);
		}
		else if (player.LastOnSlopeTime > 0)
		{
			player.StateMachine.ChangeState(player.SlopeSlideState);
		}
		else if(player.LastOnWallTime <= 0)
		{
			player.StateMachine.ChangeState(player.InAirState);
		}
		else if (InputHandler.instance.MoveInput.y != 0 && player.LastOnClimbTime > 0)
		{
			player.StateMachine.ChangeState(player.IdleClimbState);
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
