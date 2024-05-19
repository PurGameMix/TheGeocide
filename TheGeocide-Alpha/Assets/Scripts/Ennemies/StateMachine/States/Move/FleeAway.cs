using Assets.Data.Enemy.Definition;
using UnityEngine;

public class FleeAway : IState
{
    private EnemyAI _ai;

    //Physics
    public float TimeStuck;


    public FleeAway(EnemyAI enemyAI)
    {
        _ai = enemyAI;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
        TimeStuck = 0f;
    }
    public void Tick()
    {

       // PathFollow();

        //if (Vector2.Distance(_ai.rb.position, _lastPosition) <= 0f)
        //{
        //    TimeStuck += Time.deltaTime;
        //}

        //_lastPosition = _ai.rb.position;
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.Flee;
    }

    //private Vector2 GetNextDestination()
    //{
    //    _index++;
    //    if (_index >= _destinations.Length)
    //        _index = 0;

    //    return _destinations[_index].position;
    //}
}
