using Assets.Data.Upgrades.Definition;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField]
    private UpgradeChannel m_UpgradeChannel;
    [SerializeField]
    private GameStateChannel m_GameStateChannel;

    private UpgradeSO _selectedStarter;
    private int _selectionCost;
    private int _selectionLevel;

    private List<PassiveEvent> _selectedPassiveList;

    private List<ItemUpgradeEvent> _selectedItemUpgradeList;

    private int _currentUnfortunateSoulAmount = 1000; //todo get from save

    private void Awake()
    {
        _selectedPassiveList = new List<PassiveEvent>();
        _selectedItemUpgradeList = new List<ItemUpgradeEvent>();

        m_UpgradeChannel.OnStarterSelection += OnStarterSelection;
        m_UpgradeChannel.OnStarterUpgarde += OnStarterUpgrade;
        m_UpgradeChannel.OnSelectedStarterRequest += OnSelectedStarterRequest;

        m_UpgradeChannel.OnPassiveListRequest += OnPassiveListRequest;
        m_UpgradeChannel.OnPassiveUpgrade += OnPassiveUpgrade;

        m_UpgradeChannel.OnItemUpgradeListRequest += OnItemUpgradeListRequest;
        m_UpgradeChannel.OnItemUpgrade += OnItemUpgrade;

        m_GameStateChannel.OnUnfortunateSoulAmoutUpdate += OnUnfortunateSoulAmoutUpdate;
    }

    private void OnDestroy()
    {
        m_UpgradeChannel.OnStarterSelection -= OnStarterSelection;
        m_UpgradeChannel.OnStarterUpgarde -= OnStarterUpgrade;
        m_UpgradeChannel.OnSelectedStarterRequest -= OnSelectedStarterRequest;
        m_UpgradeChannel.OnPassiveListRequest -= OnPassiveListRequest;
        m_UpgradeChannel.OnPassiveUpgrade -= OnPassiveUpgrade;
        m_UpgradeChannel.OnItemUpgradeListRequest -= OnItemUpgradeListRequest;
        m_UpgradeChannel.OnItemUpgrade -= OnItemUpgrade;
        m_GameStateChannel.OnUnfortunateSoulAmoutUpdate -= OnUnfortunateSoulAmoutUpdate;
    }

    private void Start()
    {
        m_GameStateChannel.RaiseRequestUnfortunateSoulAmout();
    }

    private void OnUnfortunateSoulAmoutUpdate(int newAmount)
    {
        _currentUnfortunateSoulAmount = newAmount;
    }

    private void OnStarterUpgrade(UpgradeSO starter, int level)
    {
        var cost = starter.GetUpgradeCost(level);

        if (NotEnoughUS(cost))
        {
            Debug.Log("Pas assez");
            return;
        }

        _currentUnfortunateSoulAmount -= cost;
        m_GameStateChannel.RaiseUnfortunateSoulAmoutUpdate(_currentUnfortunateSoulAmount);
    }

    private void OnStarterSelection(UpgradeSO starter, int level)
    {
        if(starter == null)
        {
            _currentUnfortunateSoulAmount += _selectionCost;
            //Debug.Log("_selectionCost" + _selectionCost);
            _selectionCost = 0;
            _selectedStarter = null;
            _selectionLevel = 0;
        }
        else
        {
            var cost = starter.GetSelectionCost(level);
            if (NotEnoughUS(cost))
            {
                return;
            }

            _selectionCost = cost;
            //Debug.Log("_selectionCost" + _selectionCost);
            _currentUnfortunateSoulAmount -= _selectionCost;
            _selectedStarter = starter;
            _selectionLevel = level;
        }

        m_GameStateChannel.RaiseUnfortunateSoulAmoutUpdate(_currentUnfortunateSoulAmount);
    }

    private void OnSelectedStarterRequest()
    {
        m_UpgradeChannel.RaiseStarterSelection(_selectedStarter, _selectionLevel);
    }

    private void OnPassiveListRequest()
    {
        m_UpgradeChannel.RaisePassiveSelection(_selectedPassiveList);
    }

    private void OnPassiveUpgrade(UpgradeSO passive, int level)
    {
        var cost = passive.GetUpgradeCost(level);
        if (NotEnoughUS(cost))
        {
            return;
        }

        var alreadySelectedPassive = _selectedPassiveList.FirstOrDefault(item => item.GetId() == passive.GetId());

        if (alreadySelectedPassive == null)
        {
            _selectedPassiveList.Add(new PassiveEvent(passive, level));
        }
        else
        {
            alreadySelectedPassive.Level = level;
        }

        _currentUnfortunateSoulAmount -= cost;

        m_GameStateChannel.RaiseUnfortunateSoulAmoutUpdate(_currentUnfortunateSoulAmount);
        m_UpgradeChannel.RaisePassiveSelection(_selectedPassiveList);


    }


    private void OnItemUpgradeListRequest()
    {
        m_UpgradeChannel.RaiseItemUpgradeSelection(_selectedItemUpgradeList);
    }

    private void OnItemUpgrade(UpgradeSO passive, int level)
    {
        var cost = passive.GetUpgradeCost(level);
        if (NotEnoughUS(cost))
        {
            return;
        }

        var alreadySelectedItemUpgrade = _selectedItemUpgradeList.FirstOrDefault(item => item.GetId() == passive.GetId());

        if (alreadySelectedItemUpgrade == null)
        {
            _selectedItemUpgradeList.Add(new ItemUpgradeEvent(passive, level));
        }
        else
        {
            alreadySelectedItemUpgrade.Level = level;
        }

        _currentUnfortunateSoulAmount -= cost;

        m_GameStateChannel.RaiseUnfortunateSoulAmoutUpdate(_currentUnfortunateSoulAmount);
        m_UpgradeChannel.RaiseItemUpgradeSelection(_selectedItemUpgradeList);


    }
    private bool NotEnoughUS(int cost)
    {
        return _currentUnfortunateSoulAmount - cost < 0;
    }
}
