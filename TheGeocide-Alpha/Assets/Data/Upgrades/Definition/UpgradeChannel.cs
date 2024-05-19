using Assets.Data.Upgrades.Definition;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/UpgradeChannel")]
public class UpgradeChannel : ScriptableObject
{
    public delegate void RequestCallback();
    public delegate void StarterUpgradeCallback(UpgradeSO starter, int level);

    public StarterUpgradeCallback OnStarterSelection;
    public StarterUpgradeCallback OnStarterUpgarde;
    public RequestCallback OnSelectedStarterRequest;

    public delegate void PassiveSelectionCallback(List<PassiveEvent> passiveList);
    public PassiveSelectionCallback OnPassiveSelection;
    public StarterUpgradeCallback OnPassiveUpgrade;
    public RequestCallback OnPassiveListRequest;

    public delegate void ItemUpgradeSelectionCallback(List<ItemUpgradeEvent> passiveList);
    public ItemUpgradeSelectionCallback OnItemUpgradeSelection;
    public StarterUpgradeCallback OnItemUpgrade;
    public RequestCallback OnItemUpgradeListRequest;

    public void RaiseStarterSelection(UpgradeSO starter, int level)
    {
        OnStarterSelection?.Invoke(starter, level);
    }
    public void RaiseStarterUpgrade(UpgradeSO starter, int level)
    {
        OnStarterUpgarde?.Invoke(starter, level);
    }

    public void RaiseStarterSelectionRequest()
    {
        OnSelectedStarterRequest?.Invoke();
    }

    internal void RaiseRequestPassiveList()
    {
        OnPassiveListRequest?.Invoke();
    }
    internal void RaisePassiveSelection(List<PassiveEvent> passiveList)
    {
        OnPassiveSelection?.Invoke(passiveList);
    }
    internal void RaisePassiveUpgrade(UpgradeSO passive, int level)
    {
        OnPassiveUpgrade?.Invoke(passive, level);
    }

    internal void RaiseRequestItemUpgradeList()
    {
        OnItemUpgradeListRequest?.Invoke();
    }
    internal void RaiseItemUpgradeSelection(List<ItemUpgradeEvent> passiveList)
    {
        OnItemUpgradeSelection?.Invoke(passiveList);
    }
    internal void RaiseItemUpgrade(UpgradeSO passive, int level)
    {
        OnItemUpgrade?.Invoke(passive, level);
    }
}