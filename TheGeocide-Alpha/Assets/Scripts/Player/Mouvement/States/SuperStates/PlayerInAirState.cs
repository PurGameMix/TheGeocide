using Assets.Data.PlayerMouvement.Definition;

public class PlayerInAirState : PlayerState
{
	public PlayerInAirState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();

		player.SetGravityScale(data.gravityScale);
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.InAir;
    }

    public override void LogicUpdate()
	{
		//Debug.Log($"on jump {player.LastPressedJumpTime} && wallTime : {player.LastOnWallTime} && wjh : {player.player.CanWallJump()} && isFacingRIgh {player.IsFacingRight} && IsLastWallRigh {player.IsLastWallRight}");
		//Debug.Log("InputHandler.instance.ClimbInput: "+ InputHandler.instance.ClimbInput);
		base.LogicUpdate();

		if (player.LastPressedDashTime > 0 && player.DashState.CanDash())
		{
			player.StateMachine.ChangeState(player.DashState);
		}
		else if (player.LastLandingAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.LandingState);
		}
		else if (player.LastUppercutAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.UppercutState);
		}
		else if (player.LastUltimateAttackTime > 0) //Jump performed, change state
		{
			player.StateMachine.ChangeState(player.UltimateState);
		}
		else if (player._currentHealth <= 0 && player.LastFallImpactTime < 0)
		{
			player.StateMachine.ChangeState(player.DeadState);
		}
		else if (player.LastInWaterTime > 0)
		{
			player.StateMachine.ChangeState(player.IdleSwimState);
		}
		else if (player.LastOnGroundTime > 0)
		{

			if(player.LastFallPower < data.fallDamageLimit)
            {
				player.StateMachine.ChangeState(player.IdleState);
			}else if (player.IsDeadlyFall())
            {
				player.StateMachine.ChangeState(player.FallDeadState);
            }
            else
            {
                player.StateMachine.ChangeState(player.FallHurtedState);
            }
        }
		else if (player.LastOnSlopeTime > 0)
		{
			player.StateMachine.ChangeState(player.SlopeSlideState);
		}
		else if (player.isOnEdge)
        {
			player.StateMachine.ChangeState(player.EdgeClimbState);
		}
		else if(player.LastPressedJumpTime > 0 && player.LastOnWallTime > 0 && player.CanWallJump())
		{
			player.StateMachine.ChangeState(player.WallJumpState);
		}
		else if (InputHandler.instance.MoveInput.y > 0  && player.LastOnClimbTime > 0)
		{
			player.StateMachine.ChangeState(player.IdleClimbState);
		}
		else if (InputHandler.instance.MoveInput.y > 0 && player.CanCatchRope())
		{
			player.StateMachine.ChangeState(player.IdleRopeState);
		}
		else if (player.currentZipline)
		{
			player.StateMachine.ChangeState(player.ZipState);
		}
		else if (player.LastOnWallTime > 0)
		{
			player.StateMachine.ChangeState(player.WallSlideState);
		}
		else if (player.RB.velocity.y < 0)
		{
			//quick fall when holding down: feels responsive, adds some bonus depth with very little added complexity and great for speedrunners :D (In games such as Celeste and Katana ZERO)
			if (InputHandler.instance.MoveInput.y < 0)
			{
				player.SetGravityScale(data.gravityScale * data.quickFallGravityMult);
			}
			else
			{
				player.SetGravityScale(data.gravityScale * data.fallGravityMult);
			}
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

		player.Drag(data.dragAmount);
		player.Run(1);
	}		
}
