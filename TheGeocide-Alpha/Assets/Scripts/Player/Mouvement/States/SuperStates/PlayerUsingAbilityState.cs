using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerUsingAbilityState : PlayerState
{
	public PlayerUsingAbilityState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data) { }

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (InputHandler.instance.MoveInput.y > 0 && player.LastOnClimbTime > 0)
		{
			player.StateMachine.ChangeState(player.IdleClimbState);
		}
		else if (InputHandler.instance.MoveInput.y > 0 && player.CanCatchRope())
		{
			player.StateMachine.ChangeState(player.RopeState);
		}
		else if (player._currentHealth <= 0 && player.LastFallImpactTime < 0)
		{
			player.StateMachine.ChangeState(player.DeadState);
		}
		else if (player.LastWoundedTime > 0 && player.LastFallImpactTime < 0)
		{
			player.StateMachine.ChangeState(player.HurtedState);
		}
		else if (player.LastOnSlopeTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.SlopeSlideState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
