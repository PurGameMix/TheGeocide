using UnityEngine;

namespace Assets.Data.Upgrades.Definition
{
    public class PassiveEvent
    {
        internal UpgradeSO Passive;
        internal int Level;

        public PassiveEvent(UpgradeSO item, int level)
        {
            Passive = item;

            Level = level;
        }

        public PassiveEvent()
        {
        }

        public string GetId()
        {
            return Passive.GetId();
        }
        public Sprite GetIcon()
        {
            return Passive.GetIcon();
        }
    }
}