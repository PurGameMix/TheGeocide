using Assets.Data.PlayerMouvement.Definition;
using UnityEngine;

public class PlayerEdgeEvent
{
    //public Guid RequestId = Guid.NewGuid();
    public bool IsEdgeDetected;
    public Vector2 EdgePosition;

    public PlayerEdgeEvent()
    {
    }

    public PlayerEdgeEvent(bool isEdgeDetected)
    {
        IsEdgeDetected = isEdgeDetected;

    }

    public PlayerEdgeEvent(Vector2 edgePosition, bool isEdgeDetected)
    {
        EdgePosition = edgePosition;
        IsEdgeDetected = isEdgeDetected;
    }
}