using Assets.Data.PlayerMouvement.Definition;

public class PlayerClimbingState : PlayerOnClimbState
{
	public PlayerClimbingState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		//player.SetGravityScale(data.gravityScale);
		base.Exit();
		player.SetGravityScale(data.gravityScale);
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.Climb;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (InputHandler.instance.MoveInput.y == 0)
		{
			stateMachine.ChangeState(player.IdleClimbState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		//player.Drag(data.dragAmount);
		player.Climb(1);
	}
}
