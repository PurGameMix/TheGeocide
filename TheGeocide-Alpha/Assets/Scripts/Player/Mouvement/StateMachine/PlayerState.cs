using Assets.Data.PlayerMouvement.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
	protected PlayerStateMachine player;
	protected MouvementStateMachine stateMachine;
	protected PlayerMouvementSO data;

	protected float startTime;
	public bool ExitingState { get; protected set; }

	public PlayerState(PlayerStateMachine player, MouvementStateMachine stateMachine, PlayerMouvementSO data)
	{
		this.player = player;
		this.stateMachine = stateMachine;
		this.data = data;
	}

	public virtual void Enter()
	{
		startTime = Time.time;
		ExitingState = false;
	}

	public virtual void Exit()
	{
		ExitingState = true;
	}

	public virtual void LogicUpdate() {
	}

	public virtual void PhysicsUpdate() { }

	public abstract PlayerStateType GetMouvementType();
}
