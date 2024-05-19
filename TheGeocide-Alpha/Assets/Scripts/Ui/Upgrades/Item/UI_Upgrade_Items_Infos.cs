using Assets.Data.Upgrades.Definition;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade_Items_Infos : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _unfortunateSoulText;

    [SerializeField]
    private GameObject _SelectUpgradeContainer;
    [SerializeField]
    private GameObject _SelectionListPanel;

    [SerializeField]
    private GameObject _noSelectionText;

    [SerializeField]
    private GameObject _selectedItemUpgradePrefab;

    [SerializeField]
    private AudioController _audioController;

    [SerializeField]
    private UpgradeChannel _upgradeChannel;
    [SerializeField]
    private GameStateChannel _gameStateChannel;
    private UpgradeSO _data;
    private bool _hadToRefresh = false;

    //private string _NoSelectionKey = "GUI_UPGRADE_SELECTED_NONE";
    // Start is called before the first frame update
    void Awake()
    {
        _upgradeChannel.OnItemUpgradeSelection += OnItemUpgradeSelection;
        _gameStateChannel.OnUnfortunateSoulAmoutUpdate += OnUnfortunateSoulAmoutUpdate;
    }

    void OnDestroy()
    {
        _gameStateChannel.OnUnfortunateSoulAmoutUpdate -= OnUnfortunateSoulAmoutUpdate;
        _upgradeChannel.OnItemUpgradeSelection -= OnItemUpgradeSelection;
    }

    void Start()
    {
        _noSelectionText.SetActive(false);
        _hadToRefresh = true;
        _gameStateChannel.RaiseRequestUnfortunateSoulAmout();
        _upgradeChannel.RaiseRequestItemUpgradeList();
    }



    private void OnItemUpgradeSelection(List<ItemUpgradeEvent> ItemUpgradeList)
    {

        foreach (Transform child in _SelectionListPanel.transform)
        {
            if (child.gameObject.name != "NoSelectionText")
            {
                Destroy(child.gameObject);
            }
        }

        _noSelectionText.SetActive(ItemUpgradeList.Count == 0);

        foreach (var ItemUpgrade in ItemUpgradeList)
        {
            var instance = Instantiate(_selectedItemUpgradePrefab, _SelectionListPanel.GetComponent<RectTransform>());
            instance.GetComponentInChildren<Image>().sprite = ItemUpgrade.GetIcon();
        }

        _hadToRefresh = true;
    }

    private void OnUnfortunateSoulAmoutUpdate(int amount)
    {
        _unfortunateSoulText.text = $"{amount} <sprite=0>";
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
