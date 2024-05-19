using System;
using UnityEngine;

public class EffectAnimatorTransmitter : MonoBehaviour
{

    [SerializeField]
    private EntityFX effect;

    public void EffectBegin()
    {
        if(effect == null)
        {
            Debug.LogWarning($"No effect attached {transform.parent.gameObject.name}");
            return;
        }
        effect.EffectBegin(transform.parent.gameObject.name);
    }

    public void EffectCompleted()
    {
        if (effect == null)
        {
            Debug.LogWarning($"No effect attached {transform.parent.gameObject.name}");
            return;
        }
        effect.EffectCompleted(transform.parent.gameObject.name);
    }

    internal void SetEntityFX(EntityFX entityFx)
    {
        effect = entityFx;
    }
}