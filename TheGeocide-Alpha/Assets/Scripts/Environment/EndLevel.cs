using Assets.Scripts.GameManager;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _boxCollider;

    [SerializeField]
    private GameSceneChannel _gameManagerChannel;
    public SceneIndex TargetedScene;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            GoToNextLevel();
        }
    }

    private void GoToNextLevel()
    {
        _gameManagerChannel.RaiseSceneRequest(TargetedScene);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCollider.bounds.center, _boxCollider.bounds.size);
    }
}
