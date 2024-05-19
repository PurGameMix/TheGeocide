using System;
using UnityEngine;

namespace Assets.Scripts.Effects.Generic
{
    public abstract class AnimationCaller : MonoBehaviour
    {
        public abstract void AnimationBegin();
        public abstract void AnimationCompleted();
     
    }
}
