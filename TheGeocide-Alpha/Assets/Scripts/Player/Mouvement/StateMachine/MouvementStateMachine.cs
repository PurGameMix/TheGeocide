using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementStateMachine
{
	private PlayerStateMachine player;
	public PlayerState CurrentState { get; private set; }

	public PlayerState PreviousState { get; private set; }


	public void Initialize(PlayerStateMachine player, PlayerState startingState)
	{
		PreviousState = null;
		CurrentState = startingState;
		this.player = player;
		this.player.CurrentState = CurrentState.ToString();
	}

	public void ChangeState(PlayerState newState)
	{
		if (CurrentState.ExitingState)
			return;

		if(CurrentState == newState)
			return;

		CurrentState.Exit();
		PreviousState = CurrentState;
		player._playerStateChannel.RaiseOnStateExit(new PlayerStateEvent(CurrentState.GetMouvementType()));

		CurrentState = newState;
		player.CurrentState = CurrentState.ToString();

		CurrentState.Enter();
		player._playerStateChannel.RaiseOnStateEnter(new PlayerStateEvent(CurrentState.GetMouvementType()));
	}
}
