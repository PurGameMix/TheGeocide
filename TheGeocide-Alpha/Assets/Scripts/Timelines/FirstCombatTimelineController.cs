using UnityEngine;

namespace Assets.Scripts.Timelines
{
    public class FirstCombatTimelineController : TimelineController
    {

        [SerializeField]
        private EnemyIATimelineDirector _enemyDirector;
        [SerializeField]
        private Transform _skipEnemyPosition;

        [SerializeField]
        private AudioChannel _audioChannel;

        public void TurnEnemy()
        {
            _enemyDirector.LookOtherSide();
        }

        public void MoveEnemyToGoal()
        {
            _enemyDirector.MoveToClosestGoal();
        }

        public void PlaySound(string clipName)
        {
            _audioChannel.RaiseAudioRequest(new AudioEvent(clipName));
        }

        internal override void SkipTimelineActions()
        {
            _enemyDirector.TPEnemyToPosition(_skipEnemyPosition.position);
            PlaySound("Combat");
        }

        internal override void StartCutSceneActions() {}
        internal override void StopCutSceneActions() { }
    }
}