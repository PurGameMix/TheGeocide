using Assets.Data.PlayerMouvement.Definition;

public class PlayerIdleCrouchState : PlayerCrouchState
{
	public PlayerIdleCrouchState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();
		player.SetCollider(true);
	}

	public override void Exit()
	{
		base.Exit();
		player.SetCollider(false);
	}

    public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (InputHandler.instance.MoveInput.y >= 0 && _canUncrouch) //change state to runnig, when moveInput detected
		{
			stateMachine.ChangeState(player.IdleState);
		}else if (InputHandler.instance.MoveInput.x != 0) //change state to runnig, when moveInput detected
		{
			stateMachine.ChangeState(player.CrouchState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		player.Crouch(1);
	}

	public override PlayerStateType GetMouvementType()
	{
		return PlayerStateType.IdleCrouch;
	}
}
