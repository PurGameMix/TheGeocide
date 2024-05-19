using Assets.Data.Enemy.Definition;
using UnityEngine;

public class CloseAttack : IState
{
    private EnemyAI _ai;

    private Transform _target;
    public CloseAttack(EnemyAI enemyAI)
    {
        _ai = enemyAI;
    }

    public void OnEnter()
    {
        _ai._mouvementController.PlayAnimation("RepulseAttack");
        _ai._mouvementController.StopMoving();
        _target = _ai._threatDetection.GetFocusTarget(ThreatDetectionType.Close);
        _ai._mouvementController.HandleLookingSide(_ai.transform.position, _target);
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
        return EnemyStateType.AttackContact;
    }
}
