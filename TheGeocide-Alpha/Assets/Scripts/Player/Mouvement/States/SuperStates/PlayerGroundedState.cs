using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{
	public PlayerGroundedState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();

		player.DashState.ResetDashes(); //refresh Dashes upon touching the ground
		player.ResetWallJump();
	}

	public override void Exit()
	{
		base.Exit();
	}

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (player.LastPressedDashTime > 0 && player.DashState.CanDash())
		{
			player.StateMachine.ChangeState(player.DashState);
		}
		else if (player.LastUppercutAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.UppercutState);
		}
		else if (player.LastLandingAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.LandingState);
		}
		else if (player.LastUltimateAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.UltimateState);
		}
		else if (player.LastPressedJumpTime > 0)
		{
			player.StateMachine.ChangeState(player.JumpState);
		}
		else if (player.LastOnGroundTime <= 0 && player.LastOnSlopeTime <= 0 && player.LastInWaterTime <=0)
		{
			player.StateMachine.ChangeState(player.InAirState);
		}
		else if (player.LastInWaterTime > 0)
		{
			player.StateMachine.ChangeState(player.IdleSwimState);
		}
		else if (player.LastOnSlopeTime > 0)
		{
			player.StateMachine.ChangeState(player.SlopeSlideState);
		}
		else if (player._currentHealth <= 0 && player.LastFallImpactTime < 0)
		{
			player.StateMachine.ChangeState(player.DeadState);
		}
		else if (player.LastWoundedTime > 0 && player.LastFallImpactTime < 0)
		{
			player.StateMachine.ChangeState(player.HurtedState);
		}
		else if (player.LastStunTime > 0)
		{
			player.StateMachine.ChangeState(player.StunnedState);
		}
		else if (InputHandler.instance.MoveInput.y > 0 && (player.LastOnClimbTime > 0))
		{
			player.StateMachine.ChangeState(player.IdleClimbState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Drag(data.frictionAmount);
	}
}
