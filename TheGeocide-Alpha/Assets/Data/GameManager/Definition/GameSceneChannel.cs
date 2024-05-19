using Assets.Scripts.GameManager;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/GameSceneChannel")]
public class GameSceneChannel : ScriptableObject
{
    public delegate void GameManagerSceneCallback(SceneIndex levelIndex);
    public delegate void SceneRequest();
    public GameManagerSceneCallback OnSceneRequested;
    public GameManagerSceneCallback OnSceneChanged;
    public SceneRequest OnCurrentSceneRequested;
    public GameManagerSceneCallback OnCurrentSceneRespond;

    public void RaiseSceneRequest(SceneIndex sceneIndex)
    {
        //Debug.Log("RaiseSceneRequest");
        OnSceneRequested?.Invoke(sceneIndex);
    }

    public void RaiseSceneChanged(SceneIndex sceneIndex)
    {
        OnSceneChanged?.Invoke(sceneIndex);
    }

    public void RaiseCurrentSceneRequest()
    {
        OnCurrentSceneRequested?.Invoke();
    }

    public void RaiseCurrentSceneResponse(SceneIndex sceneIndex)
    {
        OnCurrentSceneRespond?.Invoke(sceneIndex);
    }
}