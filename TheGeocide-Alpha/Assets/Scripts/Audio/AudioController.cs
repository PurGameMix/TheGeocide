using Assets.Scripts.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    private Dictionary<string, Sound> _soundDico = new Dictionary<string, Sound>();

    public string Id;

    [SerializeField]
    private AudioChannel m_AudioChannel;

    public Sound[] sounds;
    private Action _waitingSoundAction;
    private AudioSource _waitingSoundCompleted;



    // Start is called before the first frame update
    void Awake()
    {

        if(m_AudioChannel != null && string.IsNullOrEmpty(Id))
        {
            Id = Guid.NewGuid().ToString();
        }

        foreach (var sound in sounds)
        {
            if (_soundDico.ContainsKey(sound.Name))
            {
                Debug.LogError($"{gameObject.name}: Audio clip with name '{sound.Name}' already exist");
                continue;
            }

            sound.Component = gameObject.AddComponent<AudioSource>();
            sound.SetComponentProperties();
            _soundDico.Add(sound.Name, sound);

            if (sound.IsAwakePlaying)
            {
                sound.Component.Play();
            }
        }

        if(m_AudioChannel != null)
        {
            m_AudioChannel.OnAudioRequested += OnAudioRequested;
        }
    }

    private void Update()
    {
        if(_waitingSoundAction == null || _waitingSoundCompleted == null)
        {
            return;
        }

        if(!_waitingSoundCompleted.isPlaying)
        {
            
            _waitingSoundAction();
            _waitingSoundCompleted = null;
            _waitingSoundAction = null;
        }
    }

    private void OnAudioRequested(AudioEvent audioEvt)
    {
        if(!string.IsNullOrEmpty(audioEvt.RequestId) && Id != audioEvt.RequestId) {
            return;
        }

        switch (audioEvt.transitionType)
        {
            case AudioTransitionType.Play: Play(audioEvt.ClipName); break;
            case AudioTransitionType.Stop: Stop(audioEvt.ClipName); break;
            case AudioTransitionType.FadeIn: StartCoroutine(FadeIn(audioEvt.ClipName, audioEvt.FadeTime)); break;
            case AudioTransitionType.FadeOut: StartCoroutine(FadeOut(audioEvt.ClipName, audioEvt.FadeTime)); break;
            case AudioTransitionType.CrossFade: CrossFade(audioEvt.ClipNameOut, audioEvt.ClipName, audioEvt.FadeTime); break;
            default: Play(audioEvt.ClipName); break;
        }

        
    }



    private void OnDestroy()
    {
        if (m_AudioChannel != null)
        {
            m_AudioChannel.OnAudioRequested -= OnAudioRequested;
        }
    }

    /// <summary>
    /// Play a sound a do action after finished
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="onAudioCompleted"></param>
    public void Play(string soundName, Action onAudioCompleted)
    {
        if (!IsValid(soundName))
        {
            return;
        }

        _waitingSoundCompleted = _soundDico[soundName].Component;
        _waitingSoundCompleted.Play();

        if (_waitingSoundCompleted.loop)
        {
            Debug.LogWarning("Sound is on loop");
        }

        _waitingSoundAction = onAudioCompleted;
       
    }

    public void Play(string soundName, bool noValidation = false)
    {
        if (!IsValid(soundName, noValidation))
        {
            return;
        }

        //Debug.Log($"Playing {soundName}");

        _soundDico[soundName].Component.Play();
    }

    public void SetVolume(string soundName, float newVolume)
    {
        if (!IsValid(soundName))
        {
            return;
        }

        //Debug.Log($"Playing {soundName}");

        _soundDico[soundName].Component.volume = newVolume;
    }

    public void CrossFade(string soundName, string newSoundName, float fadeTime = 2f)
    {
        if (!(IsValid(soundName) && IsValid(newSoundName)))
        {
            return;
        }

        StartCoroutine(_soundDico[soundName].Component.CrossFade(
            _soundDico[newSoundName].Component,
            _soundDico[newSoundName].Volume,
            fadeTime)
            );
    }

    public IEnumerator FadeIn(string soundName, float fadeTime = 2f)
    {
        if (!IsValid(soundName))
        {
            yield break;
        }

        yield return _soundDico[soundName].Component.FadeIn(fadeTime, _soundDico[soundName].Volume);
    }

    public IEnumerator FadeOut(string soundName, float fadeTime = 2f)
    {
        if (!IsValid(soundName))
        {
            yield break;
        }

        yield return _soundDico[soundName].Component.FadeOut(fadeTime);
    }

    public void UnPause(string soundName)
    {
        if (!IsValid(soundName))
        {
            return;
        }

        _soundDico[soundName].Component.UnPause();
    }

    public void Pause(string soundName)
    {
        if (!IsValid(soundName))
        {
            return;
        }

        _soundDico[soundName].Component.Pause();
    }

    public void Stop(string soundName, bool noValidation = false)
    {
        if (!IsValid(soundName, noValidation))
        {
            return;
        }

        _soundDico[soundName].Component.Stop();
    }

    internal bool IsPlaying(string soundName)
    {
        if (!IsValid(soundName))
        {
            return false;
        }

        return _soundDico[soundName].Component.isPlaying;
    }

    private bool IsValid(string soundName, bool noValidation = false)
    {
        if (!_soundDico.ContainsKey(soundName))
        {
            if (!noValidation)
            {
                Debug.LogWarning($"{gameObject.name}: Audio clip with name '{soundName}' not found");
            }
            
            return false;
        }

        return true;
    }

    internal void RegisterClip(string soundName, AudioClip audioClip, AudioMixerGroup grp = null)
    {
        if(audioClip == null)
        {
            return;
        }

        var sound = new Sound()
        {
            Clip = audioClip,
            Name = soundName,
            Pitch = 1,
            Volume = 1,
            MixerGroup = grp
        };

        sound.Component = gameObject.AddComponent<AudioSource>();
        sound.SetComponentProperties();

        if (!_soundDico.ContainsKey(soundName))
        {
            //Debug.Log("Added");
            _soundDico.Add(soundName, sound);
        }
        else
        {
            _soundDico[soundName] = sound;
        }
    }

    public string GetId()
    {
        return Id;
    }
}
