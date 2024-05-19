using Assets.Scripts.Audio;

public class AudioEvent
{
    public string RequestId = string.Empty;
    public string ClipName;
    public AudioTransitionType transitionType;

    //Use for fadeout operation
    public string ClipNameOut = string.Empty;

    public AudioEvent()
    {
    }
    public AudioEvent(string clipName)
    {
        ClipName = clipName;
    }

    public AudioEvent(string clipName, string requestId)
    {
        ClipName = clipName;
        RequestId = requestId;
    }

    public AudioEvent(AudioTransition transition, string requestId)
    {
        ClipName = transition.ClipNameEnter;
        ClipNameOut = transition.ClipNameExit;
        transitionType = transition.type;
        RequestId = requestId;
    }

    public float FadeTime = 2.0f;
}