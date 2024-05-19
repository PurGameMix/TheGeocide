namespace Assets.Data.Player.PlayerSpells.Definition
{
    public enum SpellStateType
    {
        //Preparation of a spell
        Cast,
        //Shooting instant spell or casting spell
        Release,
        //Spell is relased on a time interval
        Channel,
    }
}
