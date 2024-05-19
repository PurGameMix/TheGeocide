using System.Collections;
using UnityEngine;
namespace Assets.Scripts.Environment.Traps.Spikes
{
    public class PeriodicSpike : MecanicSpikes
    {
        public float TrapPeriodicTime;
        public float TrapShrinkTime;

        // Start is called before the first frame update
        void Start()
        {
            TrapTrigger();
        }

        private IEnumerator PeriodicSpikes()
        {
            yield return new WaitForSeconds(TrapPeriodicTime);
            TrapTrigger();
        }

        private void TrapTrigger()
        {
            _audioController.Play("SpikeTrigger");
            EmergeSpikes();
        }
        private void EmergeSpikes()
        {
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
            StartCoroutine(PeriodicSpikes());
        }
    }
}