using Assets.Data.PlayerMouvement.Definition;
using UnityEngine;

public class PlayerEdgeClimbState : PlayerState
{
	public PlayerEdgeClimbState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();
		player.SetGravityScale(0);

		var direction = player.IsFacingRight() ? 1 : -1;
		var positionoffset = new Vector2(data.edgeBeginOffset.x * direction, data.edgeBeginOffset.y);
		player.gameObject.transform.position = player.EdgePosition + positionoffset;
	}

	public override void Exit()
	{
		//player.SetGravityScale(data.gravityScale);
		base.Exit();
		var direction = player.IsFacingRight() ? 1 : -1;
		var positionoffset = new Vector2(data.edgeEndOffset.x * direction, data.edgeEndOffset.y);
		player.gameObject.transform.position = player.EdgePosition + positionoffset;
		player.SetGravityScale(data.gravityScale);
		player.EdgePosition = Vector2.zero;
		player.LastOnClimbTime = 0;
		player.LastOnWallTime = 0;
		player.LastOnGroundTime = data.coyoteTime;
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.EdgeClimb;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (!player.isOnEdge)
		{
			player.StateMachine.ChangeState(player.IdleState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		//player.Drag(data.dragAmount);
		player.RB.velocity = Vector2.zero;
	}
}
