using Assets.Data.PlayerMouvement.Definition;

public class PlayerSwimState : PlayerInWaterState
{
	public PlayerSwimState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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
		return PlayerStateType.Swim;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		 if (InputHandler.instance.MoveInput.x == 0){
			stateMachine.ChangeState(player.IdleSwimState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Swim(1);
	}
}
