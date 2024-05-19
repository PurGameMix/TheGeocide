using Assets.Data.Upgrades.Definition;
using Assets.Scripts.Ui.Upgrades;
using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrade_Items : UI_Upgrade_Panel
{

    [SerializeField]
    private List<UpgradeSO> _availableItemUpgradeList;

    private List<ItemUpgradeEvent> _userUpgradeState = new List<ItemUpgradeEvent>();

    [SerializeField]
    private UI_Upgrade_OneItem _oneItemUpgradePrefab;

    [SerializeField]
    private GameObject _contentPanel;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in _availableItemUpgradeList)
        {

            //var userItemState = _userUpgradeState.FirstOrDefault(uis => uis.GetId() == item.GetId());

            //if(userItemState == null || userItemState != null && userItemState.IsDiscovered)
            //{
                var listItem = Instantiate(_oneItemUpgradePrefab, _contentPanel.GetComponent<RectTransform>());
                listItem.PassiveData = item;
            //}
        }
    }
}
