using Assets.Data.PlayerMouvement.Definition;
using Assets.Scripts.Player.Mouvement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerUsingAbilityState
{
	private int jumpDir;

	public PlayerWallJumpState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();

		jumpDir = player.IsFacingRight() ? -1 : 1;
		player.LastWallJumpHit = player.IsFacingRight() ? WallJumpHitState.Right : WallJumpHitState.Left;
		player.WallJump(jumpDir);
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
		else if (player.LastOnGroundTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.IdleState);
		}
		else if (player.LastOnSlopeTime > 0)
        {
			player.StateMachine.ChangeState(player.SlopeSlideState);
		}
		else if (Time.time - startTime > data.wallJumpTime) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.InAirState);
		}
		//else if ((player.LastOnWallLeftTime > 0 && InputHandler.instance.MoveInput.x < 0) || (player.LastOnWallRightTime > 0 && InputHandler.instance.MoveInput.x > 0))
		//{
		//	player.StateMachine.ChangeState(player.WallSlideState);
		//}

		if ((InputHandler.instance.MoveInput.x != 0))
			player.CheckDirectionToFace(InputHandler.instance.MoveInput.x > 0);
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Drag(data.dragAmount);
		player.Run(data.wallJumpRunLerp);
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
		return PlayerStateType.WallJump;
    }
}
