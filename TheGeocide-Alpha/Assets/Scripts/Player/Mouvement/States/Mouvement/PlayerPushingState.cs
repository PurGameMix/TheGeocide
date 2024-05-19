using Assets.Data.PlayerMouvement.Definition;

public class PlayerPushingState : PlayerGroundedState
{
	public PlayerPushingState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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
		return PlayerStateType.Pushing;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(InputHandler.instance.MoveInput.x == 0)
		{
			stateMachine.ChangeState(player.IdleState);
		}else if (InputHandler.instance.MoveInput.x !=0 && player.LastOnPushTime <0)
		{
			stateMachine.ChangeState(player.RunState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Push(1);
	}
}
