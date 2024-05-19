using UnityEngine;

namespace Assets.Scripts.Environment.Traps.Spikes
{
    public class PressureSpikeAnimatorEventHandler : MonoBehaviour
    {
        [SerializeField]
        private MecanicSpikes _pressureSpike;

        // Update is called once per frame
        void Update()
        {
        }


        //Called by animator
        public void ShrinkCompleted()
        {
            _pressureSpike.ShrinkCompleted();
        }
    }
}