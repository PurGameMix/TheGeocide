using Assets.Data.PlayerMouvement.Definition;

public class PlayerCrouchWalkState : PlayerCrouchState
{
	public PlayerCrouchWalkState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
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
		return PlayerStateType.Crouch;
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		 if (InputHandler.instance.MoveInput.x == 0){
			stateMachine.ChangeState(player.IdleCrouchState);
		}else if (InputHandler.instance.MoveInput.y > 0 && _canUncrouch)
		{
			stateMachine.ChangeState(player.IdleState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Run(1);
	}
}
