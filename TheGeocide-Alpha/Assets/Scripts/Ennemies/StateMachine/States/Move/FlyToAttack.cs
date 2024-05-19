using Assets.Data.Enemy.Definition;
using System;
using System.Threading;
using UnityEngine;

public class FlyToAttack : IState
{

    private EnemyAI _ai;

    ////Stuck count
    //private Vector2 _lastPosition = Vector2.zero;
    public float TimeStuck;

    public FlyToAttack(EnemyAI ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
       // _pathFinder.StartPathing(_ai.rb, _ai.Target, OnPathComplete);
    }

    public void OnExit()
    {
        TimeStuck = 0f;
        //_pathFinder.StopPathing();
    }

    public void Tick()
    {

        //PathFollow();

        //if (Vector2.Distance(_ai.rb.position, _lastPosition) <= 0f)
        //{
        //    TimeStuck += Time.deltaTime;
        //}

        //_lastPosition = _ai.rb.position;
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.MoveToPlayer;
    }
}
