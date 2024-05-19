using Assets.Scripts.Localization;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade_OneItem : MonoBehaviour
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
    private GameObject _upgradeBtn;
    private TextMeshProUGUI _upgradeBtnTMP;

    [SerializeField]
    private AudioChannel _audioChannel;

    [SerializeField]
    private UpgradeChannel _upgradeChannel;
    [SerializeField]
    private GameStateChannel _gameStateChannel;
    private int _maxLevel = 5;

    private int _currentLevel = 0;

    void Awake()
    {
        _gameStateChannel.OnUnfortunateSoulAmoutUpdate += OnUnfortunateSoulAmoutUpdate;
    }

    void OnDestroy()
    {
        _gameStateChannel.OnUnfortunateSoulAmoutUpdate -= OnUnfortunateSoulAmoutUpdate;
    }

    private UpgradeSO _data;
    private bool _runningTransaction;

    public UpgradeSO PassiveData
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
        return 0;
    }

    #region Update Interface
    private void InitActionButtons(int currentLevel)
    {

        if (currentLevel == _maxLevel)
        {
            _upgradeBtn.SetActive(false);
            return;
        }

        _upgradeBtn.SetActive(true);
    }

    private void UpdateLevelPanel(int currentLevel)
    {
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

    private void InitToolTip(UI_ToolTipTrigger tooltipTrigger, UpgradeSO StarterSO)
    {
        tooltipTrigger.header = StarterSO.GetLocalizedTitle();
        tooltipTrigger.content = StarterSO.GetLocalizedDescription(_currentLevel);
    }

    private void UpdatePrice(int currentLevel)
    {
        var upgradePrice = _data.GetUpgradeCost(currentLevel);

        GetUpgradeButtonTMPTextCompnent().text = $"{upgradePrice}<sprite=0>";
    }

    private TextMeshProUGUI GetUpgradeButtonTMPTextCompnent()
    {
        if (_upgradeBtnTMP == null)
        {
            _upgradeBtnTMP = _upgradeBtn.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        }

        return _upgradeBtnTMP;
    }

    #endregion //Update Interface

    #region Events

    public void OnUpgrade()
    {
        _runningTransaction = true;
        _audioChannel.RaiseAudioRequest(new AudioEvent("upgrade"));

        _upgradeChannel.RaiseItemUpgrade(_data, _currentLevel);

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

        var cost = _data.GetUpgradeCost(_currentLevel);
        //Debug.Log($"Current level {_currentLevel} & cost {cost}");
        if (cost > newAmount)
        {
            _upgradeBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            _upgradeBtn.GetComponent<Button>().interactable = true;
        }
    }
    #endregion //Events
}
