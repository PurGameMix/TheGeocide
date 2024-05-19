using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Data.Upgrades.Definition
{
    public class ItemUpgradeEvent
    {
        internal UpgradeSO Upgrade;
        internal int Level;

        public bool IsDiscovered { get; set; }

        public ItemUpgradeEvent(UpgradeSO item, int level)
        {
            Upgrade = item;
            IsDiscovered = true;
            Level = level;
        }

        public ItemUpgradeEvent()
        {
        }

        public string GetId()
        {
            return Upgrade.GetId();
        }
        public Sprite GetIcon()
        {
            return Upgrade.GetIcon();
        }
    }
}