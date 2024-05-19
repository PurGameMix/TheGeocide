using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string Name;

    public AudioClip Clip;

    public AudioMixerGroup MixerGroup;

    [Range(0f, 1f)]
    public float Volume = 0.1f;

    [Range(1f, 3f)]
    public float Pitch = 1f;

    public bool IsAwakePlaying;

    public bool IsLooping;

    [Header("Spatial")]

    [Range(0f, 1f)]
    public float SpatialBlend;
    public float MinDistance = 5;
    public float MaxDistance = 20;
    public AudioRolloffMode RollOffMode;

    [HideInInspector]
    public AudioSource Component;



    internal void SetComponentProperties()
    {
        Component.clip = Clip;
        Component.outputAudioMixerGroup = MixerGroup;
        Component.volume = Volume;
        Component.pitch = Pitch;
        Component.loop = IsLooping;
        Component.playOnAwake = IsAwakePlaying;
        Component.spatialBlend = SpatialBlend;
        Component.minDistance = MinDistance;
        Component.maxDistance = MaxDistance;
        Component.rolloffMode = RollOffMode;
    }
}
