using Assets.Data.Items.Definition;
using Assets.Scripts.DataPersistence;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IDataPersistence
{
    [Header("Debug")]
    public PlayerItemSO data;

    [SerializeField]
    private PlayerInventoryChannel _playerInventoryChannel;

    [SerializeField]
    private PlayerItem _playerItemPrefab;

    private string[] _itemFolder = new string[] {"Assets/Data/Items/"};
    private Dictionary<ItemType, PlayerItemSO> _itemDico;

    private void Awake()
    {
        _itemDico = new Dictionary<ItemType, PlayerItemSO>();
    }

    private void Start()
    {
        if (data != null)
        {
            AddItem(data);
        }
    }

    private void AddItem(PlayerItemSO item)
    {
        if (_itemDico.ContainsKey(item.Type))
        {
            _itemDico[item.Type] = item;
        }
        else
        {
            _itemDico.Add(item.Type, item);
        }

        _playerInventoryChannel.RaiseInventoryChanged(item);
    }

    internal void PickUp(PlayerItemSO playerItem, Transform itemTransform)
    {
        //Debug.Log($"Has item {_inventory.HasItemInSlot(playerItem.Slot)}");

        if (HasItemInSlot(playerItem.Type))
        {

            var item = RemoveItem(playerItem.Type);
            if (!item.IsDefault)
            {
                DropItem(item, itemTransform);
            }
 
        }

        AddItem(playerItem);
    }

    private void DropItem(PlayerItemSO item, Transform itemTransform)
    {
        var instance = Instantiate(_playerItemPrefab, itemTransform.position, itemTransform.rotation);
        instance.SetData(item);
    }

    private bool HasItemInSlot(ItemType slotType)
    {
        return _itemDico.ContainsKey(slotType) && _itemDico[slotType] != null;
    }

    internal PlayerItemSO GetItem(ItemType slotType)
    {
        if (HasItemInSlot(slotType))
        {
            return _itemDico[slotType];
        }

        return null;
    }

    private PlayerItemSO RemoveItem(ItemType slotType)
    {
        var item = _itemDico[slotType];
        _itemDico[slotType] = null;
        return item;
    }

    public void LoadData(PlayerData data)
    {
        foreach(var slot in data.Inventory)
        {
            if (string.IsNullOrEmpty(slot.Value)){
                continue;
            }

            var itemSo = LoadScriptableObject(slot.Value);
            if(itemSo == null)
            {
                Debug.LogWarning($"Item so not found for {slot.Value}");
                continue;
            }
            AddItem(itemSo);
        }
    }

    public PlayerItemSO LoadScriptableObject(string itemName)
    {
        string[] guids = AssetDatabase.FindAssets(itemName, _itemFolder);
        if (guids.Length == 0)
        {
            return null;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<PlayerItemSO>(path);
    }

    public void SaveData(PlayerData data)
    {
        data.Inventory = _itemDico.ToDictionary(item => item.Key, item => item.Value.GetName());
    }
}
