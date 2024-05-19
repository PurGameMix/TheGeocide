using UnityEngine;

public class ElecExplosionAnimatorTransmitter : MonoBehaviour
{
    [SerializeField]
    private ElecExplosion effect;

    public void HandleExplosion()
    {
        effect.HandleExplosion();
    }

    public void AnimationCompleted()
    {
        effect.AnimationCompleted();
    }
}
