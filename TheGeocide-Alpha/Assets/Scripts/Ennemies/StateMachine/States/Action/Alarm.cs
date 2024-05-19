using Assets.Data.Enemy.Definition;
using UnityEngine;

public class Alarm : IState
{
    //Path path;
    private EnemyAI _ai;

    public Alarm(EnemyAI ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        _ai._mouvementController.StopMoving();
        _ai._mouvementController.PlayAnimation("Alarm");
        _ai._audioController.Play("alarmSiren");
        _ai._audioController.Play("alarmVoice");
        _ai._lastAlarmStateTime = 0;
        _ai._isAlarmCompleted = false;
        
    }

    public void OnExit()
    {
        //State interrupted by transition before animation finished
        _ai._isAlarmCompleted = true;
        _ai.BroadcastAlarm();
    }

    public void Tick()
    {
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.ActionAlarm;
    }
}
