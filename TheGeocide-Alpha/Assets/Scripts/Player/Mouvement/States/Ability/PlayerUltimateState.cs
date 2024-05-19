using Assets.Data.PlayerMouvement.Definition;
using UnityEngine;

public class PlayerUltimateState : PlayerUsingAbilityState
{
	private bool dashAttacking;

	public PlayerUltimateState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();
		//dir = Vector2.up;
		//dashAttacking = true;
		//player.Uppercut(dir);
	}

	public override void Exit()
	{
		base.Exit();
		player.SetGravityScale(data.gravityScale);

	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(Time.time - startTime > player._ultimateRecoverTime) //dashTime over transition to another state
		{
			if (player.LastOnGroundTime > 0)
				player.StateMachine.ChangeState(player.IdleState);
			else if (player.LastOnSlopeTime > 0)
				player.StateMachine.ChangeState(player.SlopeSlideState);
			else
				player.StateMachine.ChangeState(player.InAirState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.AttackUltimate;
    }
}
