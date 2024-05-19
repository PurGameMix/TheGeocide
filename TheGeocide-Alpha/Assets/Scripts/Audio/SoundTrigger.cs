using Assets.Data.Common.Definition;
using Assets.Scripts.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioTransition
{
    public AudioTransitionType type;
    public string ClipNameEnter;
    public string ClipNameExit;
}

public class SoundTrigger : BoxTriggerSide
{

    public List<AudioTransition> AudioTransitionList;

    public string AudioControllerId;

    [SerializeField]
    private AudioChannel _audioChannel;

    private void Update()
    {

        if (!IsTriggered)
        {
            return;
        }

         foreach (var transition in AudioTransitionList)
        {
            _audioChannel.RaiseAudioRequest(new AudioEvent(transition, AudioControllerId));
        }
        IsTriggered = false;
    }
}
