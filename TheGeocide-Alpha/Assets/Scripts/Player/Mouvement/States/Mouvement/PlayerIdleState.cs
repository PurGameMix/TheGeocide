using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
	public PlayerIdleState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();

		if(player.LastFallPower >= data.fallSmokeLimit)
        {
			player.PlaySmoke();
        }
	}

	public override void Exit()
	{
		base.Exit();
	}

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(InputHandler.instance.MoveInput.x != 0) //change state to runnig, when moveInput detected
		{
			stateMachine.ChangeState(player.RunState);
		}else if(InputHandler.instance.MoveInput.y < 0)
        {
			stateMachine.ChangeState(player.IdleCrouchState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Run(1);
	}

	public override PlayerStateType GetMouvementType()
	{
		return PlayerStateType.Idle;
	}
}
