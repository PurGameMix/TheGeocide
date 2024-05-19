using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/PlayerStateChannel")]
public class PlayerStateChannel : ScriptableObject
{
    public delegate void MouvementCallback(PlayerStateEvent mvtEvt);
    public delegate void SpecificStateCallback();
    public delegate void EdgeCallback(PlayerEdgeEvent edgeEvt);
    public delegate void StateEffectorCallback(StateEffectorEvent seEvt);
    public delegate void TakeDamageCallback(PlayerTakeDamageEvent tdEvt);
    public delegate void AimingCallback(AimingStateEvent tdEvt);

    public MouvementCallback OnMouvementStateEnter;
    public MouvementCallback OnMouvementStateExit;

    public SpecificStateCallback OnDeath;
    public EdgeCallback OnEdgeDetected;
    public StateEffectorCallback OnStateEffectorTriggered;

    public TakeDamageCallback OnTakeDamageRequest;

    public AimingCallback OnAimingUpdate;
    public void RaiseOnStateEnter(PlayerStateEvent newMvtEvt)
    {
        //Debug.Log($"{DateTime.UtcNow}: Enter state {newMvtEvt.Type}");
        OnMouvementStateEnter?.Invoke(newMvtEvt);
    }

    public void RaiseOnStateExit(PlayerStateEvent oldMvtEvt)
    {
        //Debug.Log($"{DateTime.UtcNow}: Exit state {oldMvtEvt.Type}");
        OnMouvementStateExit?.Invoke(oldMvtEvt);
    }

    public void RaiseOnDeath()
    {
        OnDeath?.Invoke();
    }

    public void RaiseEdgeDetection(PlayerEdgeEvent edgeEvt)
    {
        OnEdgeDetected?.Invoke(edgeEvt);
    }

    public void RaiseStateEffector(StateEffectorEvent seEvt)
    {
        OnStateEffectorTriggered?.Invoke(seEvt);
    }

    public void RaiseTakeDamageRequest(PlayerTakeDamageEvent tdEvt)
    {
        OnTakeDamageRequest?.Invoke(tdEvt);
    }

    internal void RaiseAimingUpdate(AimingStateEvent isAiming)
    {
        OnAimingUpdate.Invoke(isAiming);
    }
}