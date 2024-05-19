using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.Timelines
{
    public abstract class TimelineController : MonoBehaviour
    {
        [SerializeField]
        internal PlayableDirector _timelineDirector;

        [SerializeField]
        private FlowChannel _gameFlowChannel;
        [SerializeField]
        private FlowState _cutSceneState;
        private FlowState _cachedFlowState;

        [SerializeField]
        private DialogAgent _dialogAgent;

        [SerializeField]
        internal PlayerTimelineDirector _playerDirector;
        [SerializeField]
        private float SkipTime;

        private bool _isInDialog;
        private bool _isPaused;
        private bool _isCutSceneActive;
        void Awake()
        {
            _timelineDirector.played += CutSceneStarted;
            _timelineDirector.stopped += CutSceneStopped;
            _timelineDirector.paused += CutScenePaused;
        }

        void OnDestroy()
        {
            _timelineDirector.played -= CutSceneStarted;
            _timelineDirector.stopped -= CutSceneStopped;
            _timelineDirector.paused -= CutScenePaused;
        }

        private void Start()
        {
            InputHandler.instance.OnEscape += SkipTimeline;
        }


        internal abstract void SkipTimelineActions();
        internal abstract void StartCutSceneActions();
        internal abstract void StopCutSceneActions();

        private void SkipTimeline(InputHandler.InputArgs obj)
        {
            if (!_isCutSceneActive)
            {
                return;
            }

            if (_isInDialog)
            {
                _dialogAgent.SkipDialog();
            }

            SkipTimelineActions();

            _timelineDirector.Pause();
            _timelineDirector.time = SkipTime;
            _timelineDirector.Play();
        }

        public void DialogBegin(int parameter)
        {
            _isInDialog = true;
            _timelineDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
            _dialogAgent.OnDialogInteraction();
        }

        public void DialogEnd()
        {
            _isInDialog = false;
            _timelineDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        public void StartCutScene()
        {
            StartCutSceneActions();
            _timelineDirector.Play();
        }

        private void CutSceneStarted(PlayableDirector obj)
        {
            if (_isPaused)
            {
                _isPaused = false;
                return;
            }

            _isCutSceneActive = true;
            _playerDirector.StopMouvement();
            _cachedFlowState = FlowStateMachine.Instance.CurrentState;
            _gameFlowChannel.RaiseFlowStateRequest(_cutSceneState);
        }

        private void CutSceneStopped(PlayableDirector obj)
        {
            StopCutSceneActions();
            _gameFlowChannel.RaiseFlowStateRequest(_cachedFlowState);
            _isCutSceneActive = false;
            _cachedFlowState = null;
        }

        private void CutScenePaused(PlayableDirector obj)
        {
            _isPaused = true;
        }
    }
}
