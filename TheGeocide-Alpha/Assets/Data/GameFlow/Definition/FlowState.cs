using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameFlow/FlowState")]
public class FlowState : ScriptableObject
{  
    public bool IsInteractionPossible;
    public FlowStateType Type;
}