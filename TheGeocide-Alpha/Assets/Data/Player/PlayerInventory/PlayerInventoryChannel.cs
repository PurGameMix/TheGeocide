using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/PlayerInventoryChannel")]
public class PlayerInventoryChannel : ScriptableObject
{
    public delegate void InventoryChangedCallback(PlayerItemSO item);

    public InventoryChangedCallback OnInventoryChanged;

    public void RaiseInventoryChanged(PlayerItemSO item)
    {
        OnInventoryChanged.Invoke(item);
    }
}
