using Assets.Data.Enemy.Definition;

public class EnemyStateEvent
{
    public string EnemyId;
    public EnemyStateType Type;


    public EnemyStateEvent()
    {
    }
    public EnemyStateEvent(EnemyStateType mvtType, string enemyId)
    {
        Type = mvtType;
        EnemyId = enemyId;
    }
}