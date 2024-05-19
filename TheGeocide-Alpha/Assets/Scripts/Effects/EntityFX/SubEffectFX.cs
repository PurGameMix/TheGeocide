using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEffectFX : MonoBehaviour
{
    [SerializeField]
    private EffectAnimatorTransmitter _eat;
    public void SetMasterEffect(EntityFX masterFx)
    {
        _eat.SetEntityFX(masterFx);
    }
}
