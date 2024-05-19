using Assets.Data.PlayerMouvement.Definition;
using UnityEngine;

public class PlayerLandedState : PlayerUsingAbilityState
{

	public PlayerLandedState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(Time.time - startTime > player._landingRecoverTime) //dashTime over transition to another state
		{
			if (player.LastOnGroundTime > 0)
				player.StateMachine.ChangeState(player.IdleState);
			else if (player.LastOnSlopeTime > 0)
				player.StateMachine.ChangeState(player.SlopeSlideState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.AttackLanded;
    }
}
