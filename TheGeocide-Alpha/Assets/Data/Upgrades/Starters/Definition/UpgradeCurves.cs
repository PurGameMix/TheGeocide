using UnityEngine;

namespace Assets.Data.Upgrades.Starters.Definition
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/UpgradeCurves")]
    public class UpgradeCurves : ScriptableObject
    {
        public AnimationCurve UpgradeCostByLevel;
        public AnimationCurve SelectionCostByLevel;
        public AnimationCurve DammageByLevel;
        public AnimationCurve TimeByLevel;
        public AnimationCurve StackByLevel;
        public AnimationCurve PercentByLevel;

        //Curves are between 0-1
        //levels are between 0-5
        private const float _levelRatio = 5f;

        internal int GetSelectionCost(int currentLevel)
        {
            return (int) (SelectionCostByLevel.Evaluate(currentLevel / _levelRatio) * 5f);
        }

        internal int GetUpgradeCost(int currentLevel)
        {
            return (int) (UpgradeCostByLevel.Evaluate(currentLevel / _levelRatio) * 50f);
        }

        internal int GetDammageByLevel(int currentLevel)
        {
            return (int)(DammageByLevel.Evaluate(currentLevel / _levelRatio) * 10f);
        }

        internal int GetTimeByLevel(int currentLevel)
        {
            return (int)(TimeByLevel.Evaluate(currentLevel / _levelRatio) * 2f);
        }

        internal int GetPercentByLevel(int currentLevel)
        {
            return (int)(PercentByLevel.Evaluate(currentLevel / _levelRatio) * 20f);
        }

        internal float GetStackByLevel(int currentLevel)
        {
            return StackByLevel.Evaluate(currentLevel / _levelRatio) * 5f;
        }
    }
}