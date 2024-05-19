using UnityEngine;

namespace Assets.Scripts.Timelines
{
    public class CampBeginTimelineController : TimelineController
    {

        [SerializeField]
        private EnemyIATimelineDirector _ScoutDirector;

        [SerializeField]
        private EnemyIATimelineDirector _DonTonielDirector;

        [SerializeField]
        private Transform[] _goals;

        [SerializeField]
        private AudioChannel _audioChannel;

        public void TurnEnemy()
        {
            _ScoutDirector.LookOtherSide();
        }


        public void MoveToGoal(int index)
        {
            _DonTonielDirector.MoveToGoal(_goals[index]);
        }

        internal override void SkipTimelineActions()
        {
            _DonTonielDirector.Disable();
        }

        internal override void StartCutSceneActions()
        {
        }

        internal override void StopCutSceneActions()
        {
            _DonTonielDirector.Disable();
        }
    }
}