using Assets.Data.Items.Definition;
using Assets.Data.PlayerMouvement.Definition;
using UnityEngine;

public class PlayerUppercutState : PlayerUsingAbilityState
{
	private Vector2 dir;
	private bool dashAttacking;

	public PlayerUppercutState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{

	}

	public override void Enter()
	{
		base.Enter();

        if (player._itemMap[ItemType.Uppercut].IsDashingSpell)
        {
			dir = Vector2.up;
			dashAttacking = true;
			player.Uppercut(dir);
        }else
		{
			player.SetGravityScale(0);
		}
	}

	public override void Exit()
	{
		base.Exit();
		player.SetGravityScale(data.gravityScale);

	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(Time.time - startTime > data.dashAttackTime + data.dashEndTime) //dashTime over transition to another state
		{
			if (player.LastOnGroundTime > 0)
				player.StateMachine.ChangeState(player.IdleState);
			else if (player.LastOnSlopeTime > 0)
				player.StateMachine.ChangeState(player.SlopeSlideState);
			else
				player.StateMachine.ChangeState(player.InAirState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		if (Time.time - startTime > data.dashAttackTime)
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
		return PlayerStateType.AttackUppercut;
    }
}
