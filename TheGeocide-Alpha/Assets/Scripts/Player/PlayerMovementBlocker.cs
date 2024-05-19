using Assets.Data.Common.Definition;
using UnityEngine;

public class PlayerMovementBlocker : MonoBehaviour
{
    public CollisionBlockerSide BlockedSide;

    [SerializeField]
    private BoxCollider2D _collider;


    private PlayerStateMachine _playerRb;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
        {
            return;
        }
        if (collision.tag == "Player")
        {
            _playerRb = collision.GetComponent<PlayerStateMachine>();
            _playerRb.SetBlockage(BlockedSide);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger)
        {
            return;
        }
        if (collision.tag == "Player")
        {
            _playerRb.SetBlockage(CollisionBlockerSide.None);
            _playerRb = null;
        }
    }

    public void FixedUpdate()
    {
        if (_playerRb == null)
        {
            return;
        }

        
    }

    private Vector2 GetDrawVector(CollisionBlockerSide wantedEnterSide)
    {

        var vector = _collider.bounds.center;
        var vectorSizeH = _collider.bounds.size.y / 2;
        var vectorSizeW = _collider.bounds.size.x / 2;
        switch (wantedEnterSide)
        {
            case CollisionBlockerSide.Left: vector.x = vector.x - vectorSizeW; break;
            case CollisionBlockerSide.Right: vector.x = vector.x + vectorSizeW; break;
            case CollisionBlockerSide.Top: vector.y = vector.y + vectorSizeH; break;
            case CollisionBlockerSide.Bottom: vector.y = vector.y - vectorSizeH; break;
        }

        return vector;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);
        Gizmos.DrawLine(_collider.bounds.center, GetDrawVector(BlockedSide));
    }
}
