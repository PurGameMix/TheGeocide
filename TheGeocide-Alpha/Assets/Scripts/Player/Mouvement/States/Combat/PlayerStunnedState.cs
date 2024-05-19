using Assets.Data.PlayerMouvement.Definition;
using UnityEngine;

public class PlayerStunnedState : PlayerState
{
	public PlayerStunnedState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data) { }

	public override void Enter()
	{
		base.Enter();
		player.RB.AddForce(player._stunDirection);
	}

	public override void Exit()
	{
		base.Exit();
	}

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (player.LastStunTime < 0)
		{
			stateMachine.ChangeState(player.IdleState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override PlayerStateType GetMouvementType()
	{
		return PlayerStateType.Stun;
	}
}
