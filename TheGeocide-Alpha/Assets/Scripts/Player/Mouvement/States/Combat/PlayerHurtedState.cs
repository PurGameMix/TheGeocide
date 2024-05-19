using Assets.Data.PlayerMouvement.Definition;

public class PlayerHurtedState : PlayerWoundedState
{
	public PlayerHurtedState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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

		if (player.LastWoundedTime < 0 ) {
			stateMachine.ChangeState(stateMachine.PreviousState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override PlayerStateType GetMouvementType()
	{
		return PlayerStateType.Hurt;
	}
}
