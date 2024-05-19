using Assets.Data.PlayerMouvement.Definition;

public class PlayerIdleSwimState : PlayerInWaterState
{
	public PlayerIdleSwimState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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

		if(InputHandler.instance.MoveInput.x != 0) //change state to runnig, when moveInput detected
		{
			stateMachine.ChangeState(player.SwimState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Swim(1);
	}

	public override PlayerStateType GetMouvementType()
	{
		return PlayerStateType.IdleSwim;
	}
}
