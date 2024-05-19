using Assets.Data.Items.Definition;
using Assets.Data.Player.PlayerSpells.Definition;

public class PlayerSpellStateEvent
{
    //public Guid RequestId = Guid.NewGuid();
    public ItemType Type;
    public SpellType SpellType;

    public SpellKeyActionType Action;

    public PlayerSpellStateEvent()
    {
    }
    public PlayerSpellStateEvent(PlayerItemSO pi, SpellKeyActionType action )
    {
        if(pi != null)
        {
            Type = pi.Type;
            SpellType = pi.SpellType;
        }

        Action = action;
    }
}