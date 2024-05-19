using Assets.Scripts.Localization;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Upgrade_OneStarter : MonoBehaviour
{
    [SerializeField]
    private Image imageComponent;

    [SerializeField]
    private TextMeshProUGUI textComponent;

    [SerializeField]
    private TextMeshProUGUI descriptionComponent;

    [SerializeField]
    private GameObject _levelPanel;

    [SerializeField]
    private GameObject upgrade_empty_prefab;

    [SerializeField]
    private GameObject upgrade_full_prefab;

    [SerializeField]
    private GameObject _selectText;

    [SerializeField]
    private GameObject _selectBtn;
    private TextMeshProUGUI _selectBtnTMP;
    [SerializeField]
    private GameObject _upgradeBtn;
    private TextMeshProUGUI _upgradeBtnTMP;
    [SerializeField]
    private GameObject _unlockBtn;
    private TextMeshProUGUI _unlockBtnTMP;

    [SerializeField]
    private UpgradeChannel _upgradeChannel;

    [SerializeField]
    private GameStateChannel _gameStateChannel;

    [SerializeField]
    private AudioChannel _audioChannel;

    private int _lockedLevel = 0;

    private int _maxLevel = 5;

    private int _currentLevel = 0;

   
    private UpgradeSO _data;
    private bool _runningTransaction;

    void Awake()
    {
        _upgradeChannel.OnStarterSelection += OnStarterSelection;
        _gameStateChannel.OnUnfortunateSoulAmoutUpdate += OnUnfortunateSoulAmoutUpdate;
    }

    void OnDestroy()
    {
        _upgradeChannel.OnStarterSelection -= OnStarterSelection;
        _gameStateChannel.OnUnfortunateSoulAmoutUpdate -= OnUnfortunateSoulAmoutUpdate;
    }

    public UpgradeSO StarterData
    {
        get
        {
            return _data;
        }
        set
        {
            _data = value;
            //Debug.Log($"Debug: {value.ChoicePreview}");
            textComponent.text = value.GetLocalizedTitle();

            imageComponent.sprite = value.GetIcon();

            _currentLevel = GetCurrentLevel(value);
            UpdateTexts(_currentLevel); 
            UpdateLevelPanel(_currentLevel);
            UpdatePrice(_currentLevel);
            InitActionButtons(_currentLevel);
        }
    }

    private void UpdateTexts(int currentLevel)
    {
        descriptionComponent.text = _data.GetLocalizedDescription(_currentLevel);
        InitToolTip(textComponent.GetComponent<UI_ToolTipTrigger>(), _data);
        InitToolTip(imageComponent.GetComponent<UI_ToolTipTrigger>(), _data);
    }

    //todo get level from saves
    private int GetCurrentLevel(UpgradeSO value)
    {

        //just to be sure that there is at least one locked skill (for test)
        if (value.NameKey == "GUI_UPGRADE_STARTER_1")
        {
            return _lockedLevel;
        }
        return UnityEngine.Random.Range(_lockedLevel, _maxLevel);
    }

    #region Update Interface
    private void InitActionButtons(int currentLevel)
    {

        _selectText.SetActive(false);

        if (currentLevel == _lockedLevel)
        {
            _selectBtn.SetActive(false);
            _upgradeBtn.SetActive(false);
            _unlockBtn.SetActive(true);          
            return;
        }

        if (currentLevel == _maxLevel)
        {
            _selectBtn.SetActive(true);
            _upgradeBtn.SetActive(false);
            _unlockBtn.SetActive(false);
            return;
        }

        _selectBtn.SetActive(true);
        _upgradeBtn.SetActive(true);
        _unlockBtn.SetActive(false);
    }

    private void UpdateLevelPanel(int currentLevel)
    {

        if (currentLevel == _lockedLevel)
        {
            _levelPanel.transform.Find("LockText").gameObject.SetActive(true);
            return;
        }
        _levelPanel.transform.Find("LockText").gameObject.SetActive(false);

        foreach (Transform child in _levelPanel.transform)
        {
            if (child.gameObject.name != "LockText")
            {
                Destroy(child.gameObject);
            }
        }

        for (var i = 1; i <= _maxLevel; i++)
        {
            Instantiate(i <= currentLevel ? upgrade_full_prefab : upgrade_empty_prefab, _levelPanel.GetComponent<RectTransform>());
        }
    }

    private void InitToolTip(UI_ToolTipTrigger tooltipTrigger, UpgradeSO starterSO)
    {
        tooltipTrigger.header = starterSO.GetLocalizedTitle();
        tooltipTrigger.content = starterSO.GetLocalizedDescription(_currentLevel);
    }

    private void UpdatePrice(int currentLevel)
    {
        var selectionPrice = _data.GetSelectionCost(currentLevel);
        var upgradePrice = _data.GetUpgradeCost(currentLevel);

        if (currentLevel == _lockedLevel)
        {
            GetUnlockButtonTMPTextCompnent().text = $"{upgradePrice} <sprite=0>";
        }
        else
        {
            GetUpgradeButtonTMPTextCompnent().text = $"{upgradePrice}<sprite=0>";
        }

        GetSelectButtonTMPTextCompnent().text = $"{selectionPrice}<sprite=0>";
    }

    private TextMeshProUGUI GetUnlockButtonTMPTextCompnent()
    {
        if (_unlockBtnTMP == null)
        {
            _unlockBtnTMP = _unlockBtn.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        }

        return _unlockBtnTMP;
    }

    private TextMeshProUGUI GetUpgradeButtonTMPTextCompnent()
    {
        if (_upgradeBtnTMP == null)
        {
            _upgradeBtnTMP = _upgradeBtn.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        }

        return _upgradeBtnTMP;
    }

    private TextMeshProUGUI GetSelectButtonTMPTextCompnent()
    {
        if (_selectBtnTMP == null)
        {
            _selectBtnTMP = _selectBtn.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        }

        return _selectBtnTMP;
    }

    #endregion //Update Interface

    #region Events
    private void OnStarterSelection(UpgradeSO starter, int level)
    {
        //Si déselection ou si un autre de selectionner et qu'on est pas lock
        if (_currentLevel != _lockedLevel && (starter == null || starter.name != _data.name))
        {
            SetStarterSelected(false);
        }
    }

    public void OnSelection()
    {
        _runningTransaction = true;

        _audioChannel.RaiseAudioRequest(new AudioEvent("select"));

        _upgradeChannel.RaiseStarterSelection(_data, _currentLevel);

        SetStarterSelected(true);
        _runningTransaction = false;
    }

    private void SetStarterSelected(bool isSelected)
    {
        Color color = isSelected ? new Color(0, 0, 0, 255) : new Color(255, 255, 255, 255);
        gameObject.GetComponent<Image>().color = color;

        _selectBtn.SetActive(!isSelected);
        _upgradeBtn.SetActive(!isSelected && _currentLevel != _maxLevel);
        _selectText.SetActive(isSelected);
    }

    public void OnUpgrade()
    {
        _runningTransaction = true;

        _audioChannel.RaiseAudioRequest(new AudioEvent("upgrade"));

        if (_data.NameKey == "GUI_UPGRADE_STARTER_1")
        {
            Debug.Log(" _currentLevel++;" + _currentLevel);
        }

        _upgradeChannel.RaiseStarterUpgrade(_data, _currentLevel);
        _currentLevel++;
        UpdateLevelPanel(_currentLevel);
        UpdateTexts(_currentLevel);
        UpdatePrice(_currentLevel);

        if (_currentLevel == _maxLevel)
        {
            _upgradeBtn.SetActive(false);
        }
        _runningTransaction = false;
    }

    public void OnUnlock()
    {
        _runningTransaction = true;
        _audioChannel.RaiseAudioRequest(new AudioEvent("unlock"));

        _upgradeChannel.RaiseStarterUpgrade(_data, _currentLevel);

        _currentLevel++;

        _selectBtn.SetActive(true);
        _upgradeBtn.SetActive(true);
        _unlockBtn.SetActive(false);

        UpdateLevelPanel(_currentLevel);
        UpdatePrice(_currentLevel);
        UpdateTexts(_currentLevel);
        _runningTransaction = false;
    }

    private void OnUnfortunateSoulAmoutUpdate(int newAmount)
    {
        if (!this.isActiveAndEnabled)
        {
            return;
        }

        //On attend la fin de l'action en cours 
        //Au cas ou le changement de prix émane de l'upgrade courante
        StartCoroutine(UpdateActionButton(newAmount));
    }

    public IEnumerator UpdateActionButton(int newAmount)
    {
        yield return new WaitUntil(() => _runningTransaction == false);

        var selectionCost = _data.GetSelectionCost(_currentLevel);

        _selectBtn.GetComponent<Button>().interactable = selectionCost <= newAmount;

        var cost = _data.GetUpgradeCost(_currentLevel);
        //Debug.Log($"Current level {_currentLevel} & cost {cost}");
        if (cost > newAmount)
        {
            if (_currentLevel == _lockedLevel)
            {
                _unlockBtn.GetComponent<Button>().interactable = false;
            }
            else
            {
                _upgradeBtn.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            if (_currentLevel == _lockedLevel)
            {
                _unlockBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                _upgradeBtn.GetComponent<Button>().interactable = true;
            }
        }
    }

    private void TaskOnClick()
    {
        Debug.Log("test");
        _audioChannel.RaiseAudioRequest(new AudioEvent("disable"));
    }
    #endregion //Events
}
