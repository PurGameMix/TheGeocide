using UnityEngine;

public class ThreatObjectState
{
    public bool IsThreatDetected => ThreatDistance > 0;
    public float ThreatDistance;
    public bool IsFocus;
    public DetectionTarget Target;
    public float TargetAngle;


    public ThreatObjectState()
    {

    }

    public ThreatObjectState(string name, Transform transform)
    {
        Target = new DetectionTarget()
        {
            Name = name,
            Transform = transform
        };
    }

    public bool IsRemoved { get; internal set; }
}
