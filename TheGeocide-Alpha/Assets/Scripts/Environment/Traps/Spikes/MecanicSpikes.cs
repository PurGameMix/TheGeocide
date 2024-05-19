using System;
using UnityEngine;

namespace Assets.Scripts.Environment.Traps.Spikes
{
    public abstract class MecanicSpikes : MonoBehaviour
    {
        public int Damage = 30;

        [SerializeField]
        internal Animator _animator;

        [SerializeField]
        internal AudioController _audioController;

        [SerializeField]
        internal PressureSpikeHitBox hitBox;
        internal bool _canBeHit;


        void Update()
        {
            if (!_canBeHit)
            {
                return;
            }

            if (hitBox.HitObject != null)
            {
                var damagableObject = hitBox.HitObject.GetComponent<ICanBeDamaged>();
                if (damagableObject != null)
                {
                    damagableObject.TakeDamage(Damage, HealthEffectorType.trap);
                }
            }
        }

        public abstract void ShrinkCompleted();
    }
}
