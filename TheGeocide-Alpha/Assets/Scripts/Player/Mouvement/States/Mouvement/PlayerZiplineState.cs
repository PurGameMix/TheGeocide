using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZiplineState : PlayerState
{

	private Zipline _zipline;
	public PlayerZiplineState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
		
	}

	public override void Enter()
	{
		base.Enter();
		if (player.isOnZip)
		{
			return;
		}
		
		player.isOnZip = true;
		_zipline = player.currentZipline;
		_zipline.StartZipline(player);
	}

	public override void Exit()
	{
		//player.SetGravityScale(data.gravityScale);
		base.Exit();
		_zipline.EndZipline();
		player.CleanCurrentZipline();
		_zipline = null;
		//player.SetGravityScale(data.gravityScale);
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.OnZipline;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (InputHandler.instance.MoveInput.y <0 || _zipline.ZipOver())
		{
			//Debug.Log("hit");
			player.StateMachine.ChangeState(player.InAirState);
		}
		else if (player.LastPressedJumpTime > 0)
		{
			player.StateMachine.ChangeState(player.JumpState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
