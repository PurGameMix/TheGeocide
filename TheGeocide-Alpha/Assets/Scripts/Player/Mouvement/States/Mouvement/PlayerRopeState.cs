using Assets.Data.PlayerMouvement.Definition;
using Assets.Scripts.Player.Animations.Rigging;
using System;
using UnityEngine;

public class PlayerRopeState : PlayerState
{
	public PlayerRopeState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();

		if (!player._ropeController.IsOnRope())
		{
			player._ropeController.AttachPlayerToRope();
		}

		player._ropeController.SetConfig(RopeLayerRigConfig.Half);
		player.DashState.ResetDashes();
	}

    private void CheckHangingPosition()
    {
        throw new NotImplementedException();
    }

    public override void Exit()
	{
		//player.SetGravityScale(data.gravityScale);
		player._ropeController.SetConfig(RopeLayerRigConfig.Disable);
		player._ropeController.CancelRopeClimbing();
		base.Exit();
		//player.SetGravityScale(data.gravityScale);

	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.Rope;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (player.LastPressedJumpTime > 0)
		{
			player.StateMachine.ChangeState(player.JumpState);
			player._ropeController.DetachPlayerToRope();
		}
		else if(InputHandler.instance.MoveInput.y == 0)
		{
			stateMachine.ChangeState(player.IdleRopeState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		//player.Drag(data.dragAmount);
		player._ropeController.RopeClimb();
	}
}
