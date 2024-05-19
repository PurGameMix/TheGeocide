using Assets.Scripts.Utils;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Environment.Traps.Spikes
{
    public class PressureSpike : MecanicSpikes
    {

        public float TrapTriggerTime;
        public float TrapShrinkTime;

        [SerializeField]
        private LayerMask _canTriggerLayer;

        private bool _lock;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isInLayerMask(collision.gameObject.layer) && !_lock)
            {
                TrapTrigger();
            }
        }

        private void TrapTrigger()
        {
            _lock = true;
            _audioController.Play("SpikeTrigger");
            StartCoroutine(EmergeSpikes());
        }

        private bool isInLayerMask(int layerValue)
        {
            return _canTriggerLayer.Contains(layerValue);
        }
        private IEnumerator EmergeSpikes()
        {
            yield return new WaitForSeconds(TrapTriggerTime);
            _animator.Play("Spike_Emerge");
            _audioController.Play("SpikeIn");
            StartCoroutine(ShrinkSpikes());
            _canBeHit = true;
        }

        private IEnumerator ShrinkSpikes()
        {
            yield return new WaitForSeconds(TrapShrinkTime);
            _animator.Play("Spike_Shrink");
            _audioController.Play("SpikeOut");
        }

        public override void ShrinkCompleted()
        {
            _canBeHit = false;
            _lock = false;
        }
    }
}