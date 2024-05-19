using Assets.Scripts.Ui.Upgrades;
using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrade_Passives : UI_Upgrade_Panel
{

    [SerializeField]
    private List<UpgradeSO> _availablePassiveList;

    [SerializeField]
    private UI_Upgrade_OnePassive _oneItemPrefab;

    [SerializeField]
    private GameObject _contentPanel;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in _availablePassiveList)
        {
            var listItem = Instantiate(_oneItemPrefab, _contentPanel.GetComponent<RectTransform>());
            listItem.PassiveData = item;
        }
    }
}
