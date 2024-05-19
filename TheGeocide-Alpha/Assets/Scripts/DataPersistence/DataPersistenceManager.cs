using Assets.Scripts.GameManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]

        private const string _gameDataName = "gameData";
        private const bool _isEncryptationUsed = false;
        private const string _folderName = "saves";


        private List<IDataPersistence> _dataPersistenceObjectList;

        private GameData _gameData;
        private FileJsonDataHandler _gameDataHandler;
        private const string _gameDataExtension = ".config";

        private string _playerSaveName;
        private PlayerData _playerData;
        private FileJsonDataHandler _saveDataHandler;
        private const string _saveExtension = ".sv";

        private FileJsonDataHandler _cardDataHandler;
        private const string _saveCardExtension = ".svc";

        public static DataPersistenceManager instance { get; private set; }

        private void Awake()
        {
            if(instance != null)
            {
                Debug.LogError("Multiple instance of singleton in the scene");
            }
            instance = this;

            var directoryPath = GetFolderPath();

            _dataPersistenceObjectList = FindAllDataPersistenceObjects();

            _gameDataHandler = new FileJsonDataHandler(directoryPath, _isEncryptationUsed, _gameDataName + _gameDataExtension);
            LoadGameData();

            _saveDataHandler = new FileJsonDataHandler(directoryPath, _isEncryptationUsed, _gameData.currentSave + _saveExtension);
            _cardDataHandler = new FileJsonDataHandler(directoryPath, _isEncryptationUsed, _gameData.currentSave + _saveCardExtension);
        }

        private void Start()
        {
            SetSaveFile(_gameData.currentSave);
            LoadPlayerData();
        }

        private void OnApplicationQuit()
        {
            SavePlayerData();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            return FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>().ToList();
        }

        internal string GetCurrentSaveName()
        {
            return _gameData.currentSave;
        }

        internal void SetCurrentSave(string saveName)
        {
            _gameData.currentSave = saveName;
            SaveGameData();

            SetSaveFile(saveName);
            LoadPlayerData();
            SavePlayerData();
        }

        public void SetSaveFile(string saveName)
        {
            _playerSaveName = saveName;
            _saveDataHandler.setFile(_isEncryptationUsed, saveName + _saveExtension);
            _cardDataHandler.setFile(_isEncryptationUsed, saveName + _saveCardExtension);
        }

        #region GameData
        public void LoadGameData()
        {
            _gameData = _gameDataHandler.Load<GameData>();

            if (_gameData == null)
            {
                Debug.LogWarning("No data was found. Initializing data to defaults");
                NewsGameData();
            }
        }

        public void SaveGameData()
        {
            _gameDataHandler.Save(_gameData);
        }

        private void NewsGameData()
        {
            _gameData = new GameData();
        }
        #endregion //GameData

        #region PlayerData
        public void LoadPlayerData()
        {
            _playerData = _saveDataHandler.Load<PlayerData>();

            if (this._playerData == null)
            {
                Debug.LogWarning("No data was found. Initializing data to defaults");
                NewsPlayerData();
            }

            foreach (var dpo in _dataPersistenceObjectList)
            {
                dpo.LoadData(_playerData);
            }
        }
        public void SavePlayerData()
        {
            foreach (var dpo in _dataPersistenceObjectList)
            {
                dpo.SaveData(_playerData);
            }
            _saveDataHandler.Save(_playerData);

            var saveCard = GetCardData();
            _cardDataHandler.Save(saveCard);
        }

        internal void RemoveSave(string saveName)
        {
            if(_playerSaveName == saveName)
            {
                Debug.LogError("Save currently load, delete impossible");
                return;
            }

            File.Delete($"{GetFolderPath()}/{saveName}{_saveExtension}");
            File.Delete($"{GetFolderPath()}/{saveName}{_saveCardExtension}");
        }

        private void NewsPlayerData()
        {
            _playerData = new PlayerData();
        }
        #endregion //PlayerData

        #region PlayerCard

        /// <summary>
        /// Get info to display in save menu
        /// </summary>
        /// <returns></returns>
        private SaveCardData GetCardData()
        {
            var progression = GetProgression(_playerData.FurthestLevel);
            return new SaveCardData()
            {
                SaveName = _playerSaveName,
                FurthestLevel = _playerData.FurthestLevel,
                UnfortunateSoulAmount = _playerData.TotalUnfortunateSoulAmount,
                Progression = progression,
                LastUpdate = DateTime.UtcNow
            };
        }

        private int GetProgression(SceneIndex furthestLevel)
        {
            var startLevel = (int)SceneIndex.Hub;
            var endLevel = (int)SceneIndex.Final_lvl;
            var currentLevel = (int)furthestLevel;

            var nbLevel = endLevel - startLevel;
            var progression = currentLevel - startLevel;

            return progression * 100 / nbLevel;
        }
        #endregion //PlayerCard

        #region static method
        internal static string GetFolderPath()
        {
            return Application.persistentDataPath + "/" + _folderName;
        }

        internal static string GetCardExtension()
        {
            return _saveCardExtension;
        }


        internal static bool GetIsEncryption()
        {
            return _isEncryptationUsed;
        }
        #endregion //static method
    }
}
