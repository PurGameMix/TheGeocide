using UnityEngine;

public class IceCubeAnimatorTransmitter : MonoBehaviour
{


    [SerializeField]
    private IceCubeBuild effect;

    public void EmergeCompleted()
    {
        effect.EmergeCompleted();
    }

    public void MeltCompleted()
    {
        effect.MeltCompleted();
    }
}
