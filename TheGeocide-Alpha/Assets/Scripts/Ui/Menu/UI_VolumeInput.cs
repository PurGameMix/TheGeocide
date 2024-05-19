using Assets.Scripts.Localization;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeInput : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private AudioMixer _mixer;

    [SerializeField]
    private AudioMixerGroup _mixerGroup;

    [SerializeField]
    private TMP_Text _labelDisplayer;

    [SerializeField]
    private TMP_Text _valueDisplayer;

    [SerializeField]
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        float volume = 0;
        _mixer.GetFloat(_mixerGroup.name, out volume);
        _slider.value = volume + 80;
        _valueDisplayer.text = _slider.value.ToString();
        _labelDisplayer.text = LocalizationService.GetLocalizedString($"GUI_AUDIO_{_mixerGroup.name.ToUpperInvariant()}", TradTable.GUI);
    }

    private void ValueChangeCheck()
    {
        _mixer.SetFloat(_mixerGroup.name, _slider.value - 80);
        _valueDisplayer.text = _slider.value.ToString();

        if(_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.Play();
            StartCoroutine(StopSound());
        }
    }

    private IEnumerator StopSound()
    {
        yield return new WaitForSeconds(5);
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
