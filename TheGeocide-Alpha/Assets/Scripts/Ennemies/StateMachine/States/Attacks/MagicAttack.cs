using Assets.Data.Enemy.Definition;
using UnityEngine;

public class MagicAttack : IState
{
    private float _nextAttackTime;
    private float _attackPerSecond = 0.5f;
    private EnemyAI _ai;
    private Transform _target;

    public MagicAttack(EnemyAI ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        _target = _ai._threatDetection.GetPlayerTransform();
        _ai._mouvementController.HandleLookingSide(_ai.transform.position, _target);
    }

    public void OnExit()
    {
        _ai._mouvementController.PlayAnimation("MagicAttack");
        //State interrupted by transition before animation finished
        _ai._isAttacking = false;
    }

    public void Tick()
    {
        if (_target != null)
        {
            if (_nextAttackTime <= Time.time)
            {
                _nextAttackTime = Time.time + (1f / _attackPerSecond);
            }
        }
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.AttackBasic;
    }
}
