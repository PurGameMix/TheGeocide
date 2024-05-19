using System;
using UnityEngine;
public abstract class EntityFX : MonoBehaviour
{
    internal abstract void EffectBegin(string effectName);
    internal abstract void EffectCompleted(string effectName);
}
