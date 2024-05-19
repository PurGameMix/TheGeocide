using Assets.Data.Items.Definition;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpriteLibrary", menuName = "ScriptableObjects/ItemSpriteLibrary")]
public class ItemSpriteLibrarySO : ScriptableObject
{
    public List<ItemSetSpriteEntry> ItemSetInventoryIconList;
    public List<ItemTypeSpriteEntry> ItemTypeInventoryIconList;


    private Dictionary<ItemSetType, Sprite> _itemSetDico;
    private Dictionary<ItemType, Sprite> _itemTypeDico;

    private Dictionary<ItemSetType, Sprite> GetSetDico()
    {
        if(_itemSetDico != null)
        {
            return _itemSetDico;
        }

        _itemSetDico = new Dictionary<ItemSetType, Sprite>();
        foreach (var entry in ItemSetInventoryIconList)
        {
            _itemSetDico.Add(entry.type, entry.inventorySprite);
        }

        return _itemSetDico;
    }

    private Dictionary<ItemType, Sprite> GetTypeDico()
    {
        if (_itemTypeDico != null)
        {
            return _itemTypeDico;
        }

        _itemTypeDico = new Dictionary<ItemType, Sprite>();
        foreach (var entry in ItemTypeInventoryIconList)
        {
            _itemTypeDico.Add(entry.type, entry.inventorySprite);
        }

        return _itemTypeDico;
    }

    public Sprite GetItemSetIcon(ItemSetType type)
    {
        return GetSetDico()[type];
    }

    public Sprite GetItemTypeIcon(ItemType type)
    {
        return GetTypeDico()[type];
    }
}
