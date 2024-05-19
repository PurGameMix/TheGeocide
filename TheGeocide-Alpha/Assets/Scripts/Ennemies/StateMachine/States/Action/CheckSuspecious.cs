using Assets.Data.Enemy.Definition;
using UnityEngine;

public class CheckSuspecious : IState
{
    //Path path;
    private EnemyAI _ai;

    public CheckSuspecious(EnemyAI ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        _ai._isCheckCompleted = false;
        _ai._mouvementController.PlayAnimation("CheckArea");
    }

    public void OnExit()
    {
        var target = _ai._threatDetection.GetPlayerTransform();
        _ai._mouvementController.HandleLookingSide(_ai.transform.position, target);
        _ai._isCheckCompleted = true;
    }

    public void Tick()
    {
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.ActionCheck;
    }
}
