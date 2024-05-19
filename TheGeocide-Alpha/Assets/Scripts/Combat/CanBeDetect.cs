using UnityEngine;

public abstract class CanBeDetect : MonoBehaviour 
{
    [SerializeField]
    private Transform DetectionPoint;
    internal Transform GetDPTransform()
    {
        return DetectionPoint;
    }
}