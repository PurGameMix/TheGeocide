using Assets.Scripts.Utils;
using UnityEngine;

public class MovingColliderParenting : MonoBehaviour
{
    [SerializeField]
    private LayerMask _canFollowLayers;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_canFollowLayers.Contains(collision.gameObject.layer))
        {
            collision.transform.SetParent(transform);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_canFollowLayers.Contains(collision.gameObject.layer))
        {
            collision.transform.SetParent(null);
        }
    }
}
