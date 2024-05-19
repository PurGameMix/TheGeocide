using Assets.Scripts.DataPersistence.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

public class UI_GraphicSettings : MonoBehaviour
{

    [SerializeField]
    private TMP_Dropdown _resolutionDDL;
    private Resolution[] _availableResolution;

    [SerializeField]
    private Toggle _screenModeToggle;

    [SerializeField]
    private Slider _brightnessSlider;
    [SerializeField]
    private TMP_Text _brightnessValueDisplayer;

    [SerializeField]
    private TMP_Dropdown _qualityDDL;
    private string[] _availableQuality;

    // Start is called before the first frame update
    void Start()
    {
        SetupResolutionDDLData();

        SetupQualityDDLData();

        //setup brightness
        _brightnessSlider.value = Screen.brightness * 100;

        //ScreenMode
        _screenModeToggle.isOn = Screen.fullScreen;
    }


    public void LoadGraphicOverridesFromJson(string graphics)
    {
        GraphicSettingPrefs prefs = JsonConvert.DeserializeObject<GraphicSettingPrefs>(graphics);

        QualitySettings.SetQualityLevel(prefs.Quality);
        Screen.SetResolution(prefs.Width,prefs.Height, prefs.IsFullScreen);
        Screen.brightness = prefs.Brightness;
    }


    public string SaveGraphicSettingAsJson()
    {
        GraphicSettingPrefs prefs = new GraphicSettingPrefs
        {
            Brightness = Screen.brightness,
            IsFullScreen = Screen.fullScreen,
            Quality = QualitySettings.GetQualityLevel(),
            Width = Screen.currentResolution.width,
            Height = Screen.currentResolution.height
        };

        return JsonConvert.SerializeObject(prefs);
    }
    private void SetupResolutionDDLData()
    {
        _availableResolution = Screen.resolutions; 
        _resolutionDDL.ClearOptions();

        List<OptionData> options = new List<OptionData>();
        int currentResolutionIndex = 0;
        for (var i = 0; i < _availableResolution.Length; i++)
        {
            options.Add(new OptionData(_availableResolution[i].ToString()));
            if (Screen.currentResolution.ToString() == _availableResolution[i].ToString())
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDDL.AddOptions(options);
        _resolutionDDL.value = currentResolutionIndex;
        _resolutionDDL.RefreshShownValue();
    }



    public void SetResolution(int ddlIndex)
    {
        Screen.SetResolution(_availableResolution[ddlIndex].width, _availableResolution[ddlIndex].height, _screenModeToggle.isOn);
    }

    private void SetupQualityDDLData()
    {
        _availableQuality = QualitySettings.names;
        _qualityDDL.ClearOptions();

        List<OptionData> options = new List<OptionData>();
        int currentResolutionIndex=0;

        for (var i = 0; i < _availableQuality.Length; i++)
        {
            var tradKey = LocalizationService.GetLocalizedString($"GUI_GRAPHICS_QUALITY_{_availableQuality[i].Replace(" ", "").ToUpperInvariant()}", Assets.Scripts.Localization.TradTable.GUI);
            options.Add(new OptionData(tradKey));
            if(Screen.currentResolution.ToString() == _availableQuality[i])
            {
                currentResolutionIndex = i;
            }
        }

        _qualityDDL.AddOptions(options);
        _qualityDDL.value = currentResolutionIndex;
        _qualityDDL.RefreshShownValue();
        _qualityDDL.itemText.text = "aze";
    }

    public void SetQuality(int ddlIndex)
    {
        QualitySettings.SetQualityLevel(ddlIndex);
    }

    public void BrightnessChanged()
    {
        _brightnessValueDisplayer.text = _brightnessSlider.value.ToString();
        Screen.brightness = _brightnessSlider.value / 100;
    }

    public void ScreenModeChanged(bool isOn)
    {
        Screen.fullScreen = isOn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
