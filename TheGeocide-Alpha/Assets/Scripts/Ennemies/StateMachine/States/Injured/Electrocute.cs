using Assets.Data.Enemy.Definition;
using UnityEngine;

public class Electrocute : IState
{
    private EnemyAI _ai;
    private int _dmg = 50;
    public Electrocute(EnemyAI enemyAI)
    {
        _ai = enemyAI;
    }

    public void OnEnter()
    {
        _ai._mouvementController.PlayAnimation("Electrocute");
        _ai._audioController.Play("electrocute");
        _ai._effectsController.Play("electrocute", _ai._mouvementController.IsAgentLookingLeft());
        _ai.TakeDamage(_dmg, HealthEffectorType.playerElec);
    }

    public void OnExit()
    {
        //State interrupted by transition before animation finished
        _ai._audioController.Stop("electrocute");    
        _ai._isElectrocuted = false;
    }

    public void Tick()
    {
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.Electrocuted;
    }
}
