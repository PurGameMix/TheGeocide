using Assets.Data.Enemy.Definition;
using UnityEngine;

public class MoveToSuspecious : IState
{
    public int _pathUpdateinMs = 500;
    //Path path;
    private EnemyAI _ai;

    //Stuck count
    public float TimeStuck;
    private Vector2 _lastPosition = Vector2.zero;

    private Transform _currentGoal;
    public MoveToSuspecious(EnemyAI ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        _currentGoal = HandlePlayerLastPositionMark();
        _ai._mouvementController.Follow(_currentGoal);
        _ai._mouvementController.PlayAnimation("WalkCombat");
        _ai._mouvementController.HandleLookingSide(_ai.transform.position, _currentGoal);

        if (_ai._isAlarmTriggeredByCrew)
        {
            _ai._isAlarmTriggeredByCrew = false;
        }
    }

    public void OnExit()
    {
        _ai._threatDetection.DestroySuspeciousGoalInstance(_currentGoal);
        _currentGoal = null;

        TimeStuck = 0f;
        _ai._mouvementController.StopFollow();
    }

    public void Tick()
    {
        if (_ai._mouvementController.IsAgentStuck(_lastPosition))
        {
            TimeStuck += Time.deltaTime;
        }

        _lastPosition = _ai._mouvementController.GetAgentPosition();
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.MoveToCheck;
    }

    private Transform HandlePlayerLastPositionMark()
    {
        var playerPos = _ai._threatDetection.GetPlayerTransform();
        return _ai._threatDetection.GetSuspeciousGoalInstance(playerPos);
    }

}
