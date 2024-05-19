using Assets.Data.Common.Definition;
using Assets.Data.GameEvent.Definition;
using UnityEngine;

public class PlayerDetectedTrigger : BoxTriggerSide
{

    [SerializeField]
    private GameEventChannel _geChannel;

    private void Update()
    {
        if (!IsTriggered)
        {
            return;
        }
        
        _geChannel.RaiseEvent(new GameEvent()
        {
            Origin = gameObject,
            Type = GameEventType.PlayerDetect
        });

        IsTriggered = false;
    }

}
