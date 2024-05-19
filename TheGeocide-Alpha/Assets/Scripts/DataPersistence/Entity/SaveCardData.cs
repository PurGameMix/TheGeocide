using Assets.Scripts.GameManager;
using System;

namespace Assets.Scripts.DataPersistence
{
    [Serializable]
    public class SaveCardData
    {

        public string SaveName;
        public SceneIndex FurthestLevel = SceneIndex.Hub;
        public int UnfortunateSoulAmount = 0;
        public int Progression = 0;
        public DateTime LastUpdate = DateTime.UtcNow;

        public SaveCardData(string saveName)
        {
            SaveName = saveName;
            FurthestLevel = SceneIndex.Hub;
            UnfortunateSoulAmount = 0;
            Progression = 0;
            LastUpdate = DateTime.UtcNow;
        }
        public SaveCardData()
        {
        }
    }
}
