using Assets.Data.Enemy.Definition;
using UnityEngine;

public class MoveToAttack : IState
{
    //Pathfinding
    public int _pathUpdateinMs = 500;
    //Path path;
    private EnemyAI _ai;

    //Stuck count
    private Vector2 _lastPosition = Vector2.zero;
    public float TimeStuck;

    public MoveToAttack(EnemyAI ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        var playerTransform = _ai._threatDetection.GetPlayerTransform();
        _ai._mouvementController.Follow(playerTransform);
        _ai._mouvementController.PlayAnimation("WalkCombat");
        _ai._mouvementController.HandleLookingSide(_ai.transform.position, playerTransform);
    }

    public void OnExit()
    {
        TimeStuck = 0f;
        _ai._mouvementController.StopFollow();
    }

    public void Tick()
    {
        if (_ai._mouvementController.IsAgentStuck(_lastPosition))
        {
            TimeStuck += Time.deltaTime;
        }

        _lastPosition = _ai._mouvementController.GetAgentPosition();
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.MoveToPlayer;
    }
}
