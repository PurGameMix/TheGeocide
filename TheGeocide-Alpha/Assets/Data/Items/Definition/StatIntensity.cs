using System.Collections.Generic;

namespace Assets.Data.Items.Definition
{
    public enum StatIntensity
    {
        Low,
        Medium,
        High
    }

    public static class StatIntensityExtensions
    {
        private static Dictionary<StatIntensity, string> _colorMap = new Dictionary<StatIntensity, string>()
        {
            {StatIntensity.Low , "#2B60DE" }, //Royal Blue
            {StatIntensity.Medium , "#D5AB09" }, //Burnt Yellow
            {StatIntensity.High , "#F70D1A" }, //Ferrari Red
        };

        public static string GetColor(this StatIntensity it)
        {
            return _colorMap[it];
        }

        public static string GetTextKey(this StatIntensity it)
        {
            return "SO_INTENSITY_" + it.ToString().ToUpperInvariant();
        }
        
    }
}