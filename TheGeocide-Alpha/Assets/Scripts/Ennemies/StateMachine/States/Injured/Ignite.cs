using Assets.Data.Enemy.Definition;
using UnityEngine;

public class Ignite : IState
{
    private EnemyAI _ai;
    private float _burnCD = 1;
    private float _lastBurnTime;
    private int _dmgPerTick = 5;
    public Ignite(EnemyAI enemyAI)
    {
        _ai = enemyAI;
    }

    public void OnEnter()
    {
        _lastBurnTime = _burnCD;
        _ai._mouvementController.PlayAnimation("Ignite");
        _ai._audioController.Play("ignite");
        var effect = (TimerEntityFX) _ai._effectsController.Play("ignite", _ai._mouvementController.IsAgentLookingLeft());
        effect.StartFX(GetIgniteTime(_ai._igniteDamage));
    }

    public void OnExit()
    {
        _ai._audioController.Stop("ignite");
        //State interrupted by transition before animation finished
        _ai._igniteDamage = 0;
    }

    public void Tick()
    {
        if(_lastBurnTime <= 0)
        {
            _ai.TakeDamage(_dmgPerTick, HealthEffectorType.ignite);
            _ai._igniteDamage -= _dmgPerTick;
            _lastBurnTime = _burnCD;
        }
        _lastBurnTime -= Time.deltaTime;
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.Ignite;
    }

    private float GetIgniteTime(float damage)
    {
        return damage / _dmgPerTick;
    }
}
