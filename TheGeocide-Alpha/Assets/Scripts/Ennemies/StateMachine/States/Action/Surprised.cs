using Assets.Data.Enemy.Definition;
using UnityEngine;

public class Surprised : IState
{
    //Path path;
    private EnemyAI _ai;

    public Surprised(EnemyAI ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        _ai._mouvementController.StopMoving();
        _ai._mouvementController.PlayAnimation("Surprised");
        _ai._lastSurpriseStateTime = 0;
        _ai._isSurpriseCompleted = false;
    }

    public void OnExit()
    {
        //State interrupted by transition before animation finished
        _ai._isSurpriseCompleted = true;
    }

    public void Tick()
    {
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.ActionSuprised;
    }
}
