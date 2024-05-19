using Assets.Scripts.DataPersistence;
using Assets.Scripts.GameManager;
using Assets.Scripts.Localization;
using System.Globalization;
using TMPro;
using UnityEngine;

public class UI_OneSaveCard : MonoBehaviour
{
    
    [SerializeField]
    private TMP_Text _locationTxt;
    [SerializeField]
    private TMP_Text _unfortunateSoulTxt;
    [SerializeField]
    private TMP_Text _progressTxt;
    [SerializeField]
    private TMP_Text _lastUpdateTxt;
    public void Init(SaveCardData data) {
        _locationTxt.text = LocalizationService.GetLocalizedString(data.FurthestLevel.GetTradKey(), TradTable.GUI);
        _unfortunateSoulTxt.text = $"{data.UnfortunateSoulAmount}";
        _progressTxt.text = $"{data.Progression}%";
        _lastUpdateTxt.text = $"{data.LastUpdate.ToString("g", CultureInfo.CurrentCulture)}";
    }
}
