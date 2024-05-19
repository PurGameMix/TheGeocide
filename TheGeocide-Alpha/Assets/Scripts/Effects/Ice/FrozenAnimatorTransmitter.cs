using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenAnimatorTransmitter : MonoBehaviour
{
    [SerializeField]
    private FrozenEffect effect;

    public void FrostBegin()
    {
        effect.FrostBegin();
    }
}
