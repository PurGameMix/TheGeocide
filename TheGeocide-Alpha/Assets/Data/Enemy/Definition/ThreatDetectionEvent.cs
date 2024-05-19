using Assets.Data.Enemy.Definition;
using System;
using UnityEngine;

public class ThreatDetectionEvent
{
    public string RequestId = string.Empty;
    public ThreatDetectionType Type;
    public DetectionTarget Target;
    //Use for fadeout operation
    public string ClipNameOut = string.Empty;

    public ThreatDetectionEvent()
    {
    }

    public ThreatDetectionEvent(string requestId, ThreatDetectionType type, DetectionTarget target)
    {
        Target = target;
        RequestId = requestId;
        Type = type;
    }

    internal bool IsDetectionIn()
    {
        return Target != null;
    }

    internal bool IsDetectionOut()
    {
        return !IsDetectionIn();
    }
}