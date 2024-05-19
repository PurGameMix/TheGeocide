using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerUsingAbilityState
{
	public PlayerJumpState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();

		player.Jump();
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
		else if (player.LastLandingAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.LandingState);
		}
		else if (player.LastUppercutAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.UppercutState);
		}
		else if (player.LastUltimateAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.UltimateState);
		}
		else if (player.LastPressedJumpTime > 0 && player.LastOnWallTime > 0 && player.CanWallJump())
		{
			player.StateMachine.ChangeState(player.WallJumpState);
		}
        else if (player.LastOnSlopeTime > 0)
        {
            player.StateMachine.ChangeState(player.SlopeSlideState);
        }
        else if (player.RB.velocity.y <= 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.InAirState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Drag(data.dragAmount);
		player.Run(1);
	}

	public bool CanJumpCut()
	{
		if (player.StateMachine.CurrentState == this && player.RB.velocity.y > 0) //if the player is jumping and still moving up
			return true;
		else
			return false;
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.Jump;
    }
}
