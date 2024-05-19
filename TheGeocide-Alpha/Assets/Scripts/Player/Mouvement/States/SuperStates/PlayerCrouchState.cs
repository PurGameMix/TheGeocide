using Assets.Data.PlayerMouvement.Definition;
using UnityEngine;

public abstract class PlayerCrouchState : PlayerGroundedState
{
	public PlayerCrouchState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data) : base(player, stateMachine, data)
	{
	}

	internal bool _canUncrouch;
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

    public override PlayerStateType GetMouvementType()
    {
		return PlayerStateType.Crouch;
    }

	public override void LogicUpdate()
	{
        if (_canUncrouch)
        {
			base.LogicUpdate();
		}		
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		var crouchHeight = data.CrouchSize.y;
		var standHeight = data.StandSize.y;
		var startPos = player.transform.position + new Vector3(0, crouchHeight);
		var length = standHeight - crouchHeight;
		// Because you raycast from inside the capsule collider, it wont detect the character's collider
		var hit = Physics2D.Raycast(startPos, Vector2.up, length, player._groundLayer);
		_canUncrouch = !hit;
	}
}
