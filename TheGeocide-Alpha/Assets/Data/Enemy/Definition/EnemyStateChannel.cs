using Assets.Data.Enemy;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/EnemyStateChannel")]
public class EnemyStateChannel : ScriptableObject
{
    public delegate void StateCallback(EnemyStateEvent stateEvt);
    public delegate void FrozenCallback(FrozenStateEvent frozenEvt);
    public delegate void ElectrocuteCallback(ElectrocuteStateEvent frozenEvt);
    public delegate void DamageCallback(EnemyDamageTakenEvent dmgTakenEvt);

    public FrozenCallback OnFrozenTickChanged;
    public ElectrocuteCallback OnElectrocuteTickChanged;
    public StateCallback OnStateEnter;
    public StateCallback OnStateExit;
    public DamageCallback OnDamageTaken;

    public void RaiseOnStateEnter(EnemyStateEvent newStateEvt)
    {
        //Debug.Log($"{DateTime.UtcNow}: Enter state {newMvtEvt.Type}");
        OnStateEnter?.Invoke(newStateEvt);
    }

    public void RaiseOnStateExit(EnemyStateEvent oldStateEvt)
    {
        //Debug.Log($"{DateTime.UtcNow}: Exit state {oldMvtEvt.Type}");
        OnStateExit?.Invoke(oldStateEvt);
    }

    public void RaiseOnFrozenTickChange(FrozenStateEvent frozenEvt)
    {
        //Debug.Log($"{DateTime.UtcNow}: FrozenTick {frozenEvt.Type}");
        OnFrozenTickChanged?.Invoke(frozenEvt);
    }

    public void RaiseOnElectrocuteTickChange(ElectrocuteStateEvent frozenEvt)
    {
        //Debug.Log($"{DateTime.UtcNow}: FrozenTick {frozenEvt.Type}");
        OnElectrocuteTickChanged?.Invoke(frozenEvt);
    }
    internal void RaiseOnDamageTaken(EnemyDamageTakenEvent dmgTakenEvt)
    {
        OnDamageTaken?.Invoke(dmgTakenEvt);
    }
}