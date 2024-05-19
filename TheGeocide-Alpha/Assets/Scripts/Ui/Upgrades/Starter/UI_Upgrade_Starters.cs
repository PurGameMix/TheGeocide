using Assets.Scripts.Ui.Upgrades;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrade_Starters : UI_Upgrade_Panel
{

    [SerializeField]
    private List<UpgradeSO> _availableStarterList;

    [SerializeField]
    private UI_Upgrade_OneStarter _oneItemPrefab;

    [SerializeField]
    private GameObject _contentPanel;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in _availableStarterList)
        {
            var listItem = Instantiate(_oneItemPrefab, _contentPanel.GetComponent<RectTransform>());
            listItem.StarterData = item;
        }
    }
}
