using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerOnWallState
{
	public PlayerWallSlideState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.WallSlide;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		player.Drag(data.dragAmount);
		player.Slide();
	}
}
