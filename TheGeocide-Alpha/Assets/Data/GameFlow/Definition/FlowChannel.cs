using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/FlowChannel")]
public class FlowChannel : ScriptableObject
{
    public delegate void FlowStateCallback(FlowState state);
    public FlowStateCallback OnFlowStateRequested;
    public FlowStateCallback OnFlowStateChanged;

    public delegate void InteractionCallback();
    public InteractionCallback OnInteractionChanged;

    public void RaiseFlowStateRequest(FlowState state)
    {
        OnFlowStateRequested?.Invoke(state);
    }

    public void RaiseFlowStateChanged(FlowState state)
    {
        OnFlowStateChanged?.Invoke(state);
    }

    internal void RaiseInteractionChanged()
    {
        OnInteractionChanged?.Invoke();
    }
}