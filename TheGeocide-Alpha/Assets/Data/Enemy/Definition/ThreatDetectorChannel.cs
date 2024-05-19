using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/ThreatDetectorChannel")]
public class ThreatDetectorChannel : ScriptableObject
{
    public delegate void ThreatStateCallback(ThreatDetectionEvent detectionEvt);
    public ThreatStateCallback OnThreatDetected;

    public void RaiseDetection(ThreatDetectionEvent detectionEvt)
    {
        OnThreatDetected?.Invoke(detectionEvt);
    }
}
