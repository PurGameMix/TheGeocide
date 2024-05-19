using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/AudioChannel")]
public class AudioChannel : ScriptableObject
{
    public delegate void FlowStateCallback(AudioEvent audioEvt);
    public FlowStateCallback OnAudioRequested;

    public void RaiseAudioRequest(AudioEvent audioEvt)
    {
        OnAudioRequested?.Invoke(audioEvt);
    }
}