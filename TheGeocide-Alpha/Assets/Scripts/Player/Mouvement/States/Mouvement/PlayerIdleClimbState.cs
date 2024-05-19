using Assets.Data.PlayerMouvement.Definition;
using UnityEngine;

public class PlayerIdleClimbState : PlayerOnClimbState
{
	public PlayerIdleClimbState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();
		player.SetGravityScale(0);
	}

	public override void Exit()
	{
		//player.SetGravityScale(data.gravityScale);
		base.Exit();
		player.SetGravityScale(data.gravityScale);
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.IdleClimb;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (InputHandler.instance.MoveInput.y != 0 && player.LastOnClimbTime > 0)
		{
			player.StateMachine.ChangeState(player.ClimbState);
		}else if (InputHandler.instance.MoveInput.x != 0 && player.LastOnWallTime > 0)
		{
			player.StateMachine.ChangeState(player.WallSlideState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		//player.Drag(data.dragAmount);
		player.RB.velocity = Vector2.zero;
	}
}
