using Assets.Data.Items.Definition;
using Assets.Scripts.GameManager;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.DataPersistence
{
    [Serializable]
    public class PlayerData
    {
        public int TotalUnfortunateSoulAmount = 0;
        public int UnfortunateSoulAmount = 0;
        public int BaseHealth = 100;
        public int CurrentHealth = 100;

        public SceneIndex FurthestLevel = SceneIndex.Hub;

        public Dictionary<ItemType, string> Inventory = new Dictionary<ItemType, string>();
    }
}
