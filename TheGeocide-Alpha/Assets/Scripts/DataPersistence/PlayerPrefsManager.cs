using Assets.Scripts.DataPersistence.Entity;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerPrefsManager : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _actions;

    [SerializeField]
    private UI_GraphicSettings _graphicsUI;

    [SerializeField]
    private AudioMixer _audioMixer;


    public void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            _actions.LoadBindingOverridesFromJson(rebinds);
        }

        var graphics = PlayerPrefs.GetString("graphics");
        if (!string.IsNullOrEmpty(graphics))
        {
            _graphicsUI.LoadGraphicOverridesFromJson(graphics);
        }
    }

    private void Start()
    {
        var audios = PlayerPrefs.GetString("audios");
        if (!string.IsNullOrEmpty(audios))
        {
            LoadAudioOverridesFromJson(audios);
        }
    }

    public void OnDisable()
    {
        var rebinds = _actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);

        var audioString = SaveAudioMixAsJson();
        PlayerPrefs.SetString("audios", audioString);

        var graphicString = _graphicsUI.SaveGraphicSettingAsJson();
        PlayerPrefs.SetString("graphics", graphicString);
    }


    private void LoadAudioOverridesFromJson(string audios)
    {
        AudioMixPrefs prefs = JsonConvert.DeserializeObject<AudioMixPrefs>(audios);
        _audioMixer.SetFloat("Master", prefs.MasterVolume);
        _audioMixer.SetFloat("Music", prefs.MusicVolume);
        _audioMixer.SetFloat("Effects", prefs.EffectVolume);
        _audioMixer.SetFloat("Environment", prefs.EnvironmentVolume);
    }


    private string SaveAudioMixAsJson()
    {

        float masterVolume, musicVolume, effectVolume, environmentVolume;
        _audioMixer.GetFloat("Master", out masterVolume);
        _audioMixer.GetFloat("Music", out musicVolume);
        _audioMixer.GetFloat("Effects", out effectVolume);
        _audioMixer.GetFloat("Environment", out environmentVolume);
        AudioMixPrefs prefs = new AudioMixPrefs
        {
            MasterVolume = masterVolume,
            EffectVolume = effectVolume,
            EnvironmentVolume = environmentVolume,
            MusicVolume = musicVolume
        };

        return JsonConvert.SerializeObject(prefs);
    }
}
