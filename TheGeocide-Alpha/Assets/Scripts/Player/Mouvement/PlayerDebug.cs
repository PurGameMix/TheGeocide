using UnityEngine;
using TMPro;
using Visualization;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(PlayerStateMachine))]
public class PlayerDebug : MonoBehaviour
{
	private PlayerStateMachine _player;

	public Transform debugCanvas;
	public TextMeshProUGUI playerStateText;
	public bool DisplayArrow;
    public bool DebugVerbose;
    private VisualDrawer drawer;
	private ArrowVisual arrow;

	private string buffedState = string.Empty;

	private void Start()
	{
		_player = GetComponent<PlayerStateMachine>();

		VisualDrawer drawer = new VisualDrawer();

		if (DisplayArrow)
		{
            arrow = drawer.CreateVisual<ArrowVisual>("Vel");
            arrow.DrawVisual(Vector2.zero, Vector2.zero, Color.cyan);
        }

	}

	private void LateUpdate()
	{

		if(_player == null || _player.StateMachine.CurrentState == null || playerStateText == null || debugCanvas == null)
        {
			return;
        }

		string stateText = _player.StateMachine.CurrentState.ToString();

		stateText = stateText.Remove(0, 6);
		stateText = stateText.Remove(stateText.Length - 5, 5);
		playerStateText.text = stateText;

		if(stateText != buffedState)
        {
			if (DebugVerbose)
			{
                Debug.Log(DateTime.Now + "PlayerStateTransion : \n");
                Debug.Log(DateTime.Now + ": from => " + _player.StateMachine.PreviousState);
                Debug.Log(DateTime.Now + ": to => " + stateText);                         
            }
			buffedState = stateText;
		}

		//Debug.Log("_player.LastOnWallLeftTime : " + _player.LastOnWallLeftTime);
		//Debug.Log("_player.LastOnWallRightTime : " + _player.LastOnWallRightTime);
		//Debug.Log("_player.LastOnGroundTime : " + _player.LastOnGroundTime);

		debugCanvas.transform.position = transform.position;
		if (DisplayArrow)
		{
			arrow.MoveVisual(_player.transform.position, (Vector2)_player.transform.position + _player.RB.velocity * 0.2f);
		}
	}

    private void OnDrawGizmos()
    {
		/*
		string stateText = _player.StateMachine.CurrentState.ToString();
		stateText = stateText.Remove(0, 6);
		stateText = stateText.Remove(stateText.Length - 5, 5);

		Gizmos.DrawLine(transform.position, transform.position + (Vector3)_player.RB.velocity * 0.2f);
		*/
	}
}
