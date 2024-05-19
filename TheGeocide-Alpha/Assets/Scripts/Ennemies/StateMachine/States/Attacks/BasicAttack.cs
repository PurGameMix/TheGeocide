using Assets.Data.Enemy.Definition;
using System;
using UnityEngine;

public class BasicAttack : IState
{
    private EnemyAI _ai;

    private Transform _target;

    private bool _isAttackFollowingPlayer;
    private float _debug = 0;
    public BasicAttack(EnemyAI enemyAI, bool isAttackFollowingPlayer = false)
    {
        _ai = enemyAI;
        _isAttackFollowingPlayer = isAttackFollowingPlayer;
    }

    public void OnEnter()
    {
        _ai._mouvementController.StopMoving();
        _ai._mouvementController.PlayAnimation("BasicAttack");
        _target = _ai._threatDetection.GetFocusTarget(ThreatDetectionType.Attack);
        _ai._mouvementController.HandleLookingSide(_ai.transform.position, _target);
    }

    public void OnExit()
    {
        //State interrupted by transition before animation finished
        _ai._isAttacking = false;
    }

    public void Tick()
    {
        if (_isAttackFollowingPlayer)
        {

            var direction = (_target.position - _ai.transform.position).normalized;
            if (_ai._mouvementController.IsAgentLookingLeft() && direction.x > 0)
            {
                _ai._mouvementController.HandleLookingSide(_ai.transform.position, _target);
            }

            if (!_ai._mouvementController.IsAgentLookingLeft() && direction.x <= 0)
            {
                _ai._mouvementController.HandleLookingSide(_ai.transform.position, _target);
            }
        }

        _debug += Time.deltaTime;
        if (_debug > 3)
        {
            _debug = 0;
            Debug.Log("Stoping!");
             _ai._mouvementController.StopMoving();
            _ai._mouvementController.PlayAnimation("BasicAttack");
        }
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.AttackBasic;
    }
}
