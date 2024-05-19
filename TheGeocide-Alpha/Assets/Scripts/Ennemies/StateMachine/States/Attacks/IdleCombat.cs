using Assets.Data.Enemy.Definition;
using System;
using UnityEngine;

public class IdleCombat : IState
{
    private EnemyAI _ai;

    private Transform _target;
    public IdleCombat(EnemyAI enemyAI)
    {
        _ai = enemyAI;
    }

    public void OnEnter()
    {
        _ai._mouvementController.StopMoving();
        _target = _ai._threatDetection.GetFocusTarget(ThreatDetectionType.Attack);
        _ai._mouvementController.HandleLookingSide(_ai.transform.position, _target);
        _ai._mouvementController.PlayAnimation("IdleCombat");
    }

    public void OnExit()
    {
        //State interrupted by transition before animation finished
        _ai._isAttacking = false;
    }

    public void Tick()
    {
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.AttackIdle;
    }
}
