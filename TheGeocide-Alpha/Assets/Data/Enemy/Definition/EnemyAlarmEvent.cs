using Assets.Data.Enemy.Definition;
using UnityEngine;

public class EnemyAlarmEvent
{
    public Transform ThreatPosition;
    public EnemyStateType Type;


    public EnemyAlarmEvent()
    {
    }
    public EnemyAlarmEvent(Transform threatPosition)
    {
        ThreatPosition = threatPosition;
    }
}