using Assets.Data.Enemy;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/EnemyAlarmChannel")]
public class EnemyAlarmChannel : ScriptableObject
{
    public delegate void AlarmCallback(EnemyAlarmEvent stateEvt);

    public AlarmCallback OnAlarmTriggered;

    public void RaiseAlarm(EnemyAlarmEvent newStateEvt)
    {
        //Debug.Log($"{DateTime.UtcNow}: Enter state {newMvtEvt.Type}");
        OnAlarmTriggered?.Invoke(newStateEvt);
    }
}