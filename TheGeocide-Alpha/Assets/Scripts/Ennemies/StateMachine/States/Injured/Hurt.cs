using Assets.Data.Enemy.Definition;
using System.Collections;
using UnityEngine;

public class Hurt : IState
{
    private EnemyAI _ai;

    public Hurt(EnemyAI enemyAI)
    {
        _ai = enemyAI;
    }

    public void OnEnter()
    {
        _ai._mouvementController.PlayAnimation("Hurt");
        _ai._audioController.Play("hurt");
    }

    public void OnExit()
    {
        //State interrupted by transition before animation finished
        Debug.Log("Exit hurt state_0: " + _ai._isAttacked);
        _ai._isAttacked = false;
        Debug.Log("Exit hurt state_1: " + _ai._isAttacked);
    }

    public void Tick()
    {
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.Hurt;
    }
}
