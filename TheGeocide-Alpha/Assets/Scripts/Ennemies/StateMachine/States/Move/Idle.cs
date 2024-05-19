using Assets.Data.Enemy.Definition;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    private EnemyAI _ai;
    //Stuck count
    public float TimeStuck;

    //Patrol

    public Idle(EnemyAI ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        _ai._mouvementController.PlayAnimation("Idle");
    }

    public void OnExit()
    {
        //_ai._mouvementController.StopPatrol();
        TimeStuck = 0f;      
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
        return EnemyStateType.Idle;
    }
}
