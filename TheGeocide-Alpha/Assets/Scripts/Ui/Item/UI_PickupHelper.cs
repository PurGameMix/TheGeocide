using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PickupHelper : MonoBehaviour
{

    [SerializeField]
    private GameObject pickupPanel;

    [SerializeField]
    private GameObject equippedPanel;


    private UI_ItemHelper _pickupHelper;
    private UI_ItemHelper _equippedHelper;
    private PlayerItemSO _currentItem;
    private bool _playerHasItem;
    private GameObject _switchIcon;
    // Start is called before the first frame update
    void Awake()
    {

        _switchIcon = transform.Find("SwapItem").gameObject;
        _pickupHelper = new UI_ItemHelper(pickupPanel);

        _equippedHelper = new UI_ItemHelper(equippedPanel);

        pickupPanel.SetActive(false);
        equippedPanel.SetActive(false);
        _switchIcon.SetActive(false);
    }

    internal void SetContent(PlayerItemSO data)
    {
        _currentItem = data;

        
        _pickupHelper.SetHelperData(_currentItem);

    }

    internal void SetPlayerItemCompare(PlayerInventory playerInventory)
    {
        var equippedItem = playerInventory.GetItem(_currentItem.Type);

        _playerHasItem = equippedItem != null;

        if (_playerHasItem)
        {
            _equippedHelper.SetHelperData(equippedItem);
            
        }
    }

    internal void SetActive(bool active)
    {
        _pickupHelper._this.SetActive(active);

        if (!active)
        {
            _pickupHelper.TooltipTrigger.HideTooltip();
        }
       

        if (_playerHasItem)
        {
            _switchIcon.SetActive(active);
            _equippedHelper._this.SetActive(active);

            if (!active)
            {
                _equippedHelper.TooltipTrigger.HideTooltip();
            }
        }
    }
}
