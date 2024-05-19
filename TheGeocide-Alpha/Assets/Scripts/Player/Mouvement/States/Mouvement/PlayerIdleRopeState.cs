using Assets.Data.PlayerMouvement.Definition;
using Assets.Scripts.Player.Animations.Rigging;
using UnityEngine;

public class PlayerIdleRopeState : PlayerState
{
	public PlayerIdleRopeState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();

		if (!player._ropeController.IsOnRope())
		{
			player._ropeController.AttachPlayerToRope();
		}

		player._ropeController.SetSolversOnCurrentRope();
		player._ropeController.SetConfig(RopeLayerRigConfig.Full);
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
		return PlayerStateType.IdleRope;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (player.LastPressedJumpTime > 0)
		{
			player._ropeController.DetachPlayerToRope();
			player.StateMachine.ChangeState(player.JumpState);

		}else if (InputHandler.instance.MoveInput.y != 0 && player.IsOnRope())
		{
			//Debug.Log("hit");
			player.StateMachine.ChangeState(player.RopeState);
		}
		if (InputHandler.instance.MoveInput.x != 0 && player.IsOnRope())
		{
			//Debug.Log("hit");
			player.StateMachine.ChangeState(player.RopeSwingState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		player._ropeController.IdleRopeClimb();
		//player.RopeSwing(1);
		//player.RB.velocity = Vector2.zero;
	}
}
