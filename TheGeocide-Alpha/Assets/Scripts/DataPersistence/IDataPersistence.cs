using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DataPersistence
{
    public interface IDataPersistence
    {
        void LoadData(PlayerData data);
        void SaveData(PlayerData data);
    }
}
