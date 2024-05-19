using UnityEngine;

namespace Assets.Scripts.Timelines
{
    public class FirstCombatEndTimelineController : TimelineController
    {

        [SerializeField]
        private EnemyIATimelineDirector _enemyDirector;

        [SerializeField]
        private SoulSpawner _soulSpawner;

        [SerializeField]
        private AudioChannel _audioChannel;

        private float _xAxisLimit = 226;
        private float _playerActionDistance = 3;

        public void SpawnSoul()
        {
            _soulSpawner.Spawn(10);
        }

        public void StopSound(string clipName)
        {
            var transition = new AudioTransition()
            {
                ClipNameEnter = clipName,
                type = Audio.AudioTransitionType.Stop
            };
            _audioChannel.RaiseAudioRequest(new AudioEvent(transition, "CurrentScene"));
        }

        internal override void SkipTimelineActions()
        {          
        }

        internal override void StartCutSceneActions()
        {
            StopSound("Combat");
            Vector2 actionPosition = _enemyDirector.GetPosition();
            Vector2 playerRelativePosition;

            if (actionPosition.x < _xAxisLimit)
            {
                _playerDirector.TurnPlayer(false);
                playerRelativePosition = new Vector2(_playerActionDistance, 0);
            }
            else
            {
                playerRelativePosition = new Vector2(-_playerActionDistance, 0);
            }

            
            _soulSpawner.transform.position = new Vector2(actionPosition.x, _soulSpawner.transform.position.y);
            _playerDirector.TPPlayerToPosition(playerRelativePosition + actionPosition);

            _enemyDirector.TPEnemyToPosition(actionPosition);
        }

        internal override void StopCutSceneActions() { }
    }
}