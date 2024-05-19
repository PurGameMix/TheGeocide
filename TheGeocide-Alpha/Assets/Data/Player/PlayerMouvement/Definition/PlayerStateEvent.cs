using Assets.Data.PlayerMouvement.Definition;

public class PlayerStateEvent
{
    //public Guid RequestId = Guid.NewGuid();
    public PlayerStateType Type;


    public PlayerStateEvent()
    {
    }
    public PlayerStateEvent(PlayerStateType mvtType)
    {
        Type = mvtType;
    }
}