using UnityEngine;

public class IceBreakAnimatorTransmitter : MonoBehaviour
{
    [SerializeField]
    private IceCubeBreak effect;

    public void ExplosionBegin()
    {
        effect.ExplosionBegin();
    }

    public void EffectCompleted()
    {
        effect.EffectCompleted("");
    }
}
