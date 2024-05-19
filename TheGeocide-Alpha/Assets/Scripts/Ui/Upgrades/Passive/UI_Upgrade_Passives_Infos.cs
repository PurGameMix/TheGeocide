using Assets.Data.Upgrades.Definition;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade_Passives_Infos : MonoBehaviour
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
    private GameObject _selectedPassivePrefab;

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
         _upgradeChannel.OnPassiveSelection += OnPassiveSelection;
        _gameStateChannel.OnUnfortunateSoulAmoutUpdate += OnUnfortunateSoulAmoutUpdate;
    }

    void OnDestroy()
    {
        _gameStateChannel.OnUnfortunateSoulAmoutUpdate -= OnUnfortunateSoulAmoutUpdate;
        _upgradeChannel.OnPassiveSelection -= OnPassiveSelection;
    }

    void Start()
    {
        _noSelectionText.SetActive(false);
        _hadToRefresh = true;
        _gameStateChannel.RaiseRequestUnfortunateSoulAmout();
        _upgradeChannel.RaiseRequestPassiveList();
    }

    private void OnUnfortunateSoulAmoutUpdate(int amount)
    {
        _unfortunateSoulText.text = $"{amount} <sprite=0>";
    }

    private void OnPassiveSelection(List<PassiveEvent> passiveList)
    {

        foreach (Transform child in _SelectionListPanel.transform)
        {
            if (child.gameObject.name != "NoSelectionText")
            {
                Destroy(child.gameObject);
            }
        }

        _noSelectionText.SetActive(passiveList == null || passiveList.Count == 0);

        foreach( var passive in passiveList)
        {
            var instance = Instantiate(_selectedPassivePrefab, _SelectionListPanel.GetComponent<RectTransform>());
                instance.GetComponentInChildren<Image>().sprite = passive.GetIcon();
        }

        _hadToRefresh = true;
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
