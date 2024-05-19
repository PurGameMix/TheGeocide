namespace Assets.Data.Items.Definition
{
    public enum SpellType
    {
        Casting,
        Channel,
        Instant,
        Summon,
        Buff
    }

    public static class SpellTypeExtensions
    {
        public static bool IsCastSpell(this SpellType sp)
        {
            return sp == SpellType.Casting;
        }

        public static bool IsChannelSpell(this SpellType sp)
        {
            return sp == SpellType.Channel;
        }

        public static bool IsInstantSpell(this SpellType sp)
        {
            return sp == SpellType.Instant;
        }

        public static bool IsSummonSpell(this SpellType sp)
        {
            return sp == SpellType.Summon;
        }

        public static bool IsBuffSpell(this SpellType sp)
        {
            return sp == SpellType.Buff;
        }
    }
}