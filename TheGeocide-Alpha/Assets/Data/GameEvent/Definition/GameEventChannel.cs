using Assets.Data.GameEvent.Definition;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/GameEventChannel")]
public class GameEventChannel : ScriptableObject
{
    public delegate void GameEventCallback(GameEvent state);
    public GameEventCallback OnGameEvent;

    public void RaiseEvent(GameEvent state)
    {
        OnGameEvent?.Invoke(state);
    }
}