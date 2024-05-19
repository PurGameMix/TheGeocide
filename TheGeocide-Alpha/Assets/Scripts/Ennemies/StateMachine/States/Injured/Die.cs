using Assets.Data.Enemy.Definition;

public class Die : IState
{
    private EnemyAI _ai;

    public Die(EnemyAI enemyAI)
    {
        _ai = enemyAI;
    }

    public void OnEnter()
    {
        //_ai.rb.gravityScale = 0.5f;
        _ai._mouvementController.PlayAnimation("Die");
        _ai._audioController.Play("die");        
    }

    public void OnExit()
    {
        throw new System.Exception("Trying to leave dying state");
    }

    public void Tick()
    {
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.Die;
    }

}
