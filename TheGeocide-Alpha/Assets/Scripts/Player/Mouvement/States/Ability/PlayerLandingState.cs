using Assets.Data.Items.Definition;
using Assets.Data.PlayerMouvement.Definition;
using UnityEngine;

public class PlayerLandingState : PlayerUsingAbilityState
{
	private Vector2 dir;
	private bool dashAttacking;

	public PlayerLandingState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();

		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player_Detection"), LayerMask.NameToLayer("Enemy_spell"), true);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player_Physics"), LayerMask.NameToLayer("Enemy_spell"), true);

		if (player._itemMap[ItemType.Landing].IsDashSpell())
		{
			dir = Vector2.down;
			dashAttacking = true;
			player.Uppercut(dir);
        }
        else
        {
			player.SetGravityScale(0);
		}
	}

	public override void Exit()
	{
		base.Exit();

		player.SetGravityScale(data.gravityScale);

		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player_Detection"), LayerMask.NameToLayer("Enemy_spell"), false);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player_Physics"), LayerMask.NameToLayer("Enemy_spell"), false);
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		
			

		if (Time.time - startTime > data.landingAttackTime + data.landingEndTime) //dashTime over transition to another state
		{
				player.StateMachine.ChangeState(player.InAirState);
        }
        else if (player.LastOnGroundTime > 0 && player._itemMap[ItemType.Landing].HasLandingEffect())
		{
			player.StateMachine.ChangeState(player.LandedState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		if (Time.time - startTime > data.landingAttackTime)
		{
			//initial dash phase over, now begin slowing down and giving control back to player
			player.Drag(data.dragAmount);
			player.Run(data.dashEndRunLerp); //able to apply some run force but will be limited (~50% of normal)

			if(dashAttacking)
				StopDash();
		}
		else
		{
			player.Drag(data.dashAttackDragAmount);
		}
	}

	private void StopDash()
    {
		dashAttacking = false;
		player.SetGravityScale(data.gravityScale);

		if (dir.y > 0)
		{
			if (dir.x == 0)
				player.RB.AddForce(Vector2.down * player.RB.velocity.y * (1 - data.dashUpEndMult), ForceMode2D.Impulse);
			else
				player.RB.AddForce(Vector2.down * player.RB.velocity.y * (1 - data.dashUpEndMult) * .7f, ForceMode2D.Impulse);
		}
	}

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.AttackLanding;
    }
}
