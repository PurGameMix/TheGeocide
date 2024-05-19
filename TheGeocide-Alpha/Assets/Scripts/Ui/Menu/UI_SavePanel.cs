using Assets.Scripts.DataPersistence;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_SavePanel : MonoBehaviour
{
    [SerializeField]
    private UI_DeleteSaveModal _deleteModal;
    private string _deleteName;

    [SerializeField]
    private Button NewSaveButton;

    [SerializeField]
    private UI_OneSaveCard _oneItemPrefab;

    [SerializeField]
    private GameObject _contentPanel;

    private Dictionary<string, Button> _saveList = new Dictionary<string, Button>();
    private int _maxSaveNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        _deleteModal.gameObject.SetActive(false);

        var _availableSDCList = GetSaveDataCardList();

        if(_availableSDCList.Count == 0)
        {
            CreateNewSave();
            return;
        }

        foreach (var saveCard in _availableSDCList)
        {
            CreateSaveCardUI(saveCard);
        }

        var selectedSave = DataPersistenceManager.instance.GetCurrentSaveName();
        DataPersistenceManager.instance.SetCurrentSave(selectedSave);
        _saveList[selectedSave].Select();
    }

    private void CreateSaveCardUI(SaveCardData saveCard)
    {
        var oneItem = Instantiate(_oneItemPrefab, _contentPanel.GetComponent<RectTransform>());
        oneItem.Init(saveCard);

        var selectButton = oneItem.GetComponent<Button>();
        selectButton.onClick.AddListener(delegate { SelectSave(saveCard.SaveName);});
        var deleteButton = oneItem.transform.Find("DeleteBtn").GetComponent<Button>();
        deleteButton.onClick.AddListener(delegate { DeleteSave(saveCard.SaveName); });
        _saveList.Add(saveCard.SaveName, selectButton);
    }

    private List<SaveCardData> GetSaveDataCardList()
    {
        var builtList = new List<SaveCardData>();
        string directoryPath = DataPersistenceManager.GetFolderPath();
        DirectoryInfo dir = new DirectoryInfo(directoryPath);
        var fileExtension = DataPersistenceManager.GetCardExtension();
        FileInfo[] info = dir.GetFiles($"*{fileExtension}");
        

        if (info.Length == 0)
        {
            return builtList;
        }

        _maxSaveNumber = info.Select(item => int.Parse(item.Name.Replace("save", string.Empty)?.Replace(fileExtension, string.Empty))).ToList().Max();
        foreach (FileInfo f in info)
        {
            builtList.Add(LoadGameCardData(f));
        }

        return builtList.OrderBy(item => item.LastUpdate).ToList();
    }

    public SaveCardData LoadGameCardData(FileInfo f)
    {
        var readHandler = new FileJsonDataHandler(f.DirectoryName, DataPersistenceManager.GetIsEncryption(), f.Name);

        var data = readHandler.Load<SaveCardData>();

        if (data == null)
        {
            Debug.LogWarning("No data was found. Initializing data to defaults");
            return new SaveCardData();
        }

        return data;
    }

    #region Click events

    public void CreateNewSave()
    {
        _maxSaveNumber++;
        var saveName = $"save{_maxSaveNumber}";
        CreateSaveCardUI(new SaveCardData(saveName));  
        DataPersistenceManager.instance.SetCurrentSave(saveName);
        _saveList[saveName].Select();
    }

    public void SelectSave(string saveName)
    {
        DataPersistenceManager.instance.SetCurrentSave(saveName);
    }

    public void DeleteSave(string saveName)
    {
        _deleteName = saveName;
        DisplayModal(true);    
    }

    public void DisplayModal(bool isEnable)
    {
        _deleteModal.gameObject.SetActive(isEnable);
        foreach (var kvp in _saveList)
        {
            kvp.Value.interactable = !isEnable;
        }
        NewSaveButton.interactable = !isEnable;

        if (!isEnable)
        {
            _saveList[DataPersistenceManager.instance.GetCurrentSaveName()].Select();
        }
    }

    public void ConfirmDeleteSave()
    {
        var selectedSave = DataPersistenceManager.instance.GetCurrentSaveName();
       
        Destroy(_saveList[_deleteName].gameObject);
        _saveList.Remove(_deleteName);

        if (_deleteName == selectedSave)
        {
            var newSelection = _saveList.Last().Key;
            DataPersistenceManager.instance.SetCurrentSave(newSelection);
            _saveList[_deleteName].Select();
        }


        DataPersistenceManager.instance.RemoveSave(_deleteName);
        _deleteName = string.Empty;
        DisplayModal(false);
    }

    public void CancelDeleteSave()
    {
        _deleteName = string.Empty;
        DisplayModal(false);
    }

    #endregion //Click events
}
