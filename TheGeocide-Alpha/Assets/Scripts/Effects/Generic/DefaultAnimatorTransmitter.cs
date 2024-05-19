using System;
using System.Collections.Generic;
namespace Assets.Scripts.Effects.Generic
{
    using System;
    using UnityEngine;

    public class DefaultAnimatorTransmitter : MonoBehaviour
    {

        [SerializeField]
        private AnimationCaller entity;

        public void AnimationBegin()
        {
            if (entity == null)
            {
                Debug.LogWarning($"No effect object {transform.parent.gameObject.name}");
                return;
            }
            entity.AnimationBegin();
        }

        public void AnimationCompleted()
        {
            if (entity == null)
            {
                Debug.LogWarning($"No effect object {transform.parent.gameObject.name}");
                return;
            }
            entity.AnimationCompleted();
        }
    }
}
