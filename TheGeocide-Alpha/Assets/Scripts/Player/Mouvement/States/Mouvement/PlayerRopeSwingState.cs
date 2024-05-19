using Assets.Data.PlayerMouvement.Definition;
using Assets.Scripts.Player.Animations.Rigging;
using UnityEngine;

public class PlayerRopeSwingState : PlayerState
{
	public PlayerRopeSwingState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();

		if (!player._ropeController.IsOnRope())
		{
			player._ropeController.AttachPlayerToRope();
		}

		player._ropeController.SetConfig(RopeLayerRigConfig.Front);
	}

	public override void Exit()
	{
		//player.SetGravityScale(data.gravityScale);
		base.Exit();
		player._ropeController.SetConfig(RopeLayerRigConfig.Disable);
		//player.SetGravityScale(data.gravityScale);
	}

	public override PlayerStateType GetMouvementType()
	{
		return PlayerStateType.RopeSwing;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (player.LastPressedJumpTime > 0)
		{
			player._ropeController.DetachPlayerToRope();
			player.StateMachine.ChangeState(player.JumpState);
		}
		else if (InputHandler.instance.MoveInput.y != 0 && player.IsOnRope())
		{
			//Debug.Log("hit");
			player.StateMachine.ChangeState(player.RopeState);
		}
		else if(InputHandler.instance.MoveInput.y == 0 && InputHandler.instance.MoveInput.x == 0 && player.IsOnRope())
		{
			//Debug.Log("hit");
			player.StateMachine.ChangeState(player.IdleRopeState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		player.RopeSwing(1);
		//player.RB.velocity = Vector2.zero;
	}
}
