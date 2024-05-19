using Assets.Data.PlayerMouvement.Definition;

public abstract class PlayerInWaterState : PlayerState
{
	public PlayerInWaterState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();
		player.SetGravityScale(data.waterGravityScale);
	}

	public override void Exit()
	{
		base.Exit();
		player.SetGravityScale(data.gravityScale);
	}

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (player.LastPressedDashTime > 0 && player.DashState.CanDash())
		{
			player.StateMachine.ChangeState(player.DashState);
		}
		else if (player.LastUppercutAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.UppercutState);
		}
		else if (player.LastLandingAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.LandingState);
		}
		else if (player.LastUltimateAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.UltimateState);
		}
		else if (player.LastPressedJumpTime > 0)
		{
			player.StateMachine.ChangeState(player.JumpState);
		}
		else if (player.LastOnGroundTime <= 0 && player.LastOnSlopeTime <= 0 && player.LastInWaterTime <= 0)
		{
			player.StateMachine.ChangeState(player.InAirState);
		}
		else if (player.LastOnGroundTime > 0 && player.LastInWaterTime < 0)
		{
			player.StateMachine.ChangeState(player.IdleState);
		}
		else if (player.LastOnSlopeTime > 0)
		{
			player.StateMachine.ChangeState(player.SlopeSlideState);
		}
		else if (player._currentHealth <= 0 && player.LastFallImpactTime < 0)
		{
			player.StateMachine.ChangeState(player.DeadState);
		}
		else if (player.LastWoundedTime > 0 && player.LastFallImpactTime < 0)
		{
			player.StateMachine.ChangeState(player.HurtedState);
		}
		else if (player.LastStunTime > 0)
		{
			player.StateMachine.ChangeState(player.StunnedState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}		
}
