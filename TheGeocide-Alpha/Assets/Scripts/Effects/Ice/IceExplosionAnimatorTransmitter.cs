using UnityEngine;

public class IceExplosionAnimatorTransmitter : MonoBehaviour
{
    [SerializeField]
    private IceExplosion effect;

    public void HandleExplosion()
    {
        effect.HandleExplosion();
    }

    public void AnimationCompleted()
    {
        effect.AnimationCompleted();
    }
}
