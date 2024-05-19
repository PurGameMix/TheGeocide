using Assets.Data.Enemy.Definition;

public interface IState
{
    void Tick();
    void OnEnter();
    void OnExit();
    EnemyStateType GetStateType();
}