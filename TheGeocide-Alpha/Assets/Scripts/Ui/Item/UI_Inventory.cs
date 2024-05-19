using Assets.Data.Items.Definition;
using Assets.Scripts.Ui.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{

    private Dictionary<ItemType,PlayerItemSO> _inventoryDico;

    [SerializeField]
    private GameObject _inventoryPanel;

    [SerializeField]
    public Animator _uiAnimator;

    private bool _isGuiOpen;

    [SerializeField]
    private List<UI_ItemEntry> _slotList;


    [SerializeField]
    private AudioChannel _guiAudioChannel;

    [SerializeField]
    private PlayerInventoryChannel _playerInventoryChannel;
    [SerializeField]
    private FlowChannel _flowChannel;

    private Dictionary<ItemType, UI_ItemEntry> _imageComponentMap = new Dictionary<ItemType, UI_ItemEntry>();
    private FlowStateType _currentFlowState;
    private void Awake()
    {
        _inventoryDico = new Dictionary<ItemType, PlayerItemSO>();
        _playerInventoryChannel.OnInventoryChanged += OnInventoryChanged;
        _flowChannel.OnFlowStateChanged += OnFlowStateChanged;
    }

    private void OnDestroy()
    {
        _playerInventoryChannel.OnInventoryChanged -= OnInventoryChanged;
        _flowChannel.OnFlowStateChanged -= OnFlowStateChanged;
    }

    private void Start()
    {
        _isGuiOpen = false;
        _inventoryPanel.SetActive(false);

        foreach (var entry in _slotList)
        {
            _imageComponentMap.Add(entry.ItemType, entry);
        }
        InputHandler.instance.OnInventory += OnInventoryKeyPressed;
    }


    private void OnFlowStateChanged(FlowState state)
    {
        _currentFlowState = state.Type;
    }

    private void OnInventoryKeyPressed(InputHandler.InputArgs obj)
    {
        if(_currentFlowState != FlowStateType.InGame)
        {
            return;
        }

        Debug.Log("Registered ");
        if (_isGuiOpen)
        {
            CloseInventoryPanel();
            return;
        }

        OpenInventoryPanel();
    }

    private void CloseInventoryPanel()
    {
        _uiAnimator.Play("Inventory_OUT");
        _guiAudioChannel.RaiseAudioRequest(new AudioEvent("ClosePanel"));
        
        CancelTooltipTrigger();
        _isGuiOpen = false;

        StartCoroutine(DelayPanelDisable());
    }

    private IEnumerator DelayPanelDisable()
    {
        yield return new WaitForSeconds(1);
        _inventoryPanel.SetActive(false);

    }

    private void OpenInventoryPanel()
    {
        _uiAnimator.Play("Inventory_IN");
        _isGuiOpen = true;
        RefreshInventoryItems();
        _inventoryPanel.SetActive(true);
        _guiAudioChannel.RaiseAudioRequest(new AudioEvent("OpenPanel"));
    }

    private void OnInventoryChanged(PlayerItemSO item)
    {
        if (_inventoryDico.ContainsKey(item.Type))
        {
            _inventoryDico[item.Type] = item;
            return;
        }

        _inventoryDico.Add(item.Type, item);
    }

    private void CancelTooltipTrigger()
    {
        foreach (var kvp in _imageComponentMap)
        {

            var slot = _imageComponentMap[kvp.Key];
            //updating texts
            var tooltipTrigger = slot.ToolTipComponent;
            tooltipTrigger.HideTooltip();
        }
    }

    private void RefreshInventoryItems()
    {
        foreach (var kvp in _imageComponentMap)
        {

            var slot = _imageComponentMap[kvp.Key];
            //updating picture
            var imgC = slot.ImageComponent;
            //updating texts
            var tooltipTrigger = slot.ToolTipComponent;

            if (_inventoryDico.ContainsKey(kvp.Key) && _inventoryDico[kvp.Key] != null)
            {
                var itemSo = _inventoryDico[kvp.Key];
                imgC.enabled = true;
                imgC.sprite = itemSo.GetSetSprite();

                tooltipTrigger.header = itemSo.GetDetailHeader();
                tooltipTrigger.content = itemSo.GetDetailContent();
                tooltipTrigger.enabled = true;
            }
            else
            {
                imgC.enabled = false;
                tooltipTrigger.enabled = false;
            }
        }
    }
}
