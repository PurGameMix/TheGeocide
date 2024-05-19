using Assets.Scripts.Localization;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade_Starters_Infos : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _unfortunateSoulText;

    [SerializeField]
    private GameObject _SelectUpgradeContainer;

    [SerializeField]
    private GameObject _upgradeImage;

    [SerializeField]
    private TextMeshProUGUI upgradeTitle;

    [SerializeField]
    private GameObject _unSelectBtn;

    [SerializeField]
    private AudioController _audioController;

    [SerializeField]
    private UpgradeChannel _upgradeChannel;
    private UpgradeSO _data;

    [SerializeField]
    private GameStateChannel _gameStateChannel;

    private bool _hadToRefresh = false;

    private string _NoSelectionKey = "GUI_UPGRADE_SELECTED_NONE";
    // Start is called before the first frame update
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
    void Start()
    {
        _unSelectBtn.SetActive(false);
        upgradeTitle.text = LocalizationService.GetLocalizedString(_NoSelectionKey, TradTable.GUI);
        _upgradeImage.SetActive(false);
        _hadToRefresh = true;
        _gameStateChannel.RaiseRequestUnfortunateSoulAmout();
    }
    private void OnStarterSelection(UpgradeSO starter, int level)
    {
        if (starter == null)
        {
            return;
        }

        _data = starter;

        upgradeTitle.text = starter.GetLocalizedTitle();

        _upgradeImage.SetActive(true);
        _upgradeImage.GetComponent<Image>().sprite = starter.GetIcon();
        _unSelectBtn.SetActive(true);
        _hadToRefresh = true;
    }

    private void OnUnfortunateSoulAmoutUpdate(int amount)
    {
        _unfortunateSoulText.text = $"{amount} <sprite=0>";
    }

    public void OnUnselection()
    {
        _data = null;
        _audioController.Play("unselect");
        _unSelectBtn.SetActive(false);
        upgradeTitle.text = LocalizationService.GetLocalizedString(_NoSelectionKey, TradTable.GUI);
        _upgradeImage.SetActive(false);

        _upgradeChannel.RaiseStarterSelection(null, 0);
    }

    private void FixedUpdate()
    {
        if (_hadToRefresh)
        {
            //Debug.Log("ForceRefresh");
            StartCoroutine(RefreshLayout());
        }
    }

    IEnumerator RefreshLayout()
    {
        yield return new WaitForFixedUpdate();

        VerticalLayoutGroup vlg = _SelectUpgradeContainer.GetComponent<VerticalLayoutGroup>();
        vlg.enabled = false;
        vlg.enabled = true;

        _hadToRefresh = false;
    }
}
