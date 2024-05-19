using Assets.Scripts.DataPersistence;
using Assets.Scripts.GameManager;
using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour, IDataPersistence
{
    //player
    private int _unfortunateSoulPlayerTotal;
    private int _unfortunateSoulPlayerAmount;

    private SceneIndex _currentPlayerLevel;
    private SceneIndex _furthestPlayerLevel;

    private int _basePlayerHealth;
    private int _currentPlayerHealth;

    private bool _isLoaded;
    private bool _hpAsked;

    [SerializeField]
    private GameStateChannel _gStateC;

    [SerializeField]
    private GameSceneChannel _gSceneC;
    

    private void Awake()
    {
        _gStateC.OnUnfortunateSoulAmoutRequested += OnUnfortunateSoulAmoutRequested;
        _gStateC.OnUnfortunateSoulAddition += OnUnfortunateSoulAddition;
        _gStateC.OnHealthPointRequested += OnHealthPointRequested;

        _gSceneC.OnSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        _gStateC.OnUnfortunateSoulAmoutRequested -= OnUnfortunateSoulAmoutRequested;
        _gStateC.OnUnfortunateSoulAddition -= OnUnfortunateSoulAddition;

        _gSceneC.OnSceneChanged -= OnSceneChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_hpAsked)
        {
            return;
        }

        if (_isLoaded)
        {        
            _gStateC.RaisedHealthPointAnswer(_basePlayerHealth, _currentPlayerHealth);
            _hpAsked = false;
        }
    }

    #region Events

    private void OnUnfortunateSoulAddition(int newAmount)
    {
        if(newAmount > 0)
        {
            _unfortunateSoulPlayerTotal += newAmount;
        }
        
        _unfortunateSoulPlayerAmount += newAmount;
        _gStateC.RaiseUnfortunateSoulAmoutUpdate(_unfortunateSoulPlayerAmount);
    }

    private void OnUnfortunateSoulAmoutRequested()
    {
        _gStateC.RaiseUnfortunateSoulAmoutUpdate(_unfortunateSoulPlayerAmount);
    }

    private void OnHealthPointRequested()
    {
        if (!_isLoaded)
        {
            _hpAsked = true;
            return;
        }

        _gStateC.RaisedHealthPointAnswer(_basePlayerHealth, _currentPlayerHealth);
    }

    private void OnSceneChanged(SceneIndex levelIndex)
    {
        _currentPlayerLevel = levelIndex;
        if(_furthestPlayerLevel < levelIndex)
        {
            _furthestPlayerLevel = levelIndex;
        }
    }

    #endregion //Events


    public void SaveData(PlayerData data)
    {
        data.UnfortunateSoulAmount = _unfortunateSoulPlayerAmount;
        data.TotalUnfortunateSoulAmount = _unfortunateSoulPlayerTotal;
        data.BaseHealth = _basePlayerHealth;
        data.CurrentHealth = _currentPlayerHealth;
        data.FurthestLevel = _furthestPlayerLevel;
    }

    public void LoadData(PlayerData data)
    {
        _unfortunateSoulPlayerAmount = data.UnfortunateSoulAmount;
        _unfortunateSoulPlayerTotal = data.TotalUnfortunateSoulAmount;
        _basePlayerHealth = data.BaseHealth;
        _currentPlayerHealth = data.CurrentHealth;
        _furthestPlayerLevel = data.FurthestLevel;

        _isLoaded = true;
        _gStateC.RaiseUnfortunateSoulAmoutUpdate(_unfortunateSoulPlayerAmount);
    }
}
