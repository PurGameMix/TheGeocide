using System;
using UnityEngine;

public class ActionFXAnimatorTransmitter : MonoBehaviour
{

    [SerializeField]
    private ActionFX effect;

    private string _name;


    private void Awake()
    {
        _name = transform.gameObject.name;
    }

    public void EffectBegin()
    {
        if(effect == null)
        {
            Debug.LogWarning($"No effect attached {_name}");
            return;
        }
        effect.EffectBegin(_name);
    }

    public void ActionTrigger()
    {
        if (effect == null)
        {
            Debug.LogWarning($"No effect attached {_name}");
            return;
        }
        effect.ActionTriggered(_name);
    }

    public void EffectCompleted()
    {
        if (effect == null)
        {
            Debug.LogWarning($"No effect attached {_name}");
            return;
        }
        effect.EffectCompleted(_name);
    }
}