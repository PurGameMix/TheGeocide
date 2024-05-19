using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlopeSlideState : PlayerGroundedState
{
	public PlayerSlopeSlideState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
		
	}

	private float _direction;

	public override void Enter()
	{
		base.Enter();

		var origin = player._slopeCheckPoint.position + Vector3.up;

         var hit = Physics2D.Raycast(origin, Vector2.down, 2, player._slopLayer);
		var angle = Vector2.SignedAngle(Vector2.up, hit.normal);
		_direction = angle > 0 ? -1 : 1;
	}

	public override void Exit()
	{
		base.Exit();
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.SlopeSlide;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (player.LastOnSlopeTime < 0)
		{
			//Debug.Log("hit");
			player.StateMachine.ChangeState(player.IdleState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.SlopeSlide(1, _direction);
	}
}
