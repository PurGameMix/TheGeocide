namespace Assets.Scripts.Environment.Breakables
{
    public enum BreakableMaterialType
    {
        Rock,
        Wood
    }

    public static class BreakableMaterialTypeExtensions
    {
        public static string GetBreakClipName(this BreakableMaterialType bmt)
        {
            return $"{bmt}_Break";
        }

        public static string GetHitClipName(this BreakableMaterialType bmt)
        {
            return $"{bmt}_Hit";
        }
    }
}
