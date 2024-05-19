using Assets.Data.PlayerMouvement.Definition;

public class PlayerRunState : PlayerGroundedState
{
	public PlayerRunState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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
		return PlayerStateType.Run;
    }

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (InputHandler.instance.MoveInput.x == 0)
		{
			stateMachine.ChangeState(player.IdleState);
		}
		else if (player.LastOnPushTime >= 0 && !player.IsAiming())
		{
			stateMachine.ChangeState(player.PushingState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Run(1);
	}
}
