using UnityEngine;
using PathBerserker2d;
using System;
using Assets.Data.Enemy.Definition;

namespace Assets.Scripts.PathBerserker2dExtentions
{
    /// <summary>
    /// Let the agent patrol through the given goals in list order.
    /// </summary>
    public class EnemyIAMouvement : MonoBehaviour
    {
        [SerializeField]
        private EnemyStateChannel _aiStateChannel;

        [SerializeField]
        private NavAgent _navAgent;

        [SerializeField]
        private TransformBasedMovement _tbMvt;
        public float RunningFactor;
        private EnemyStateType _currentState;
        private bool _isInCombat;
        private bool _isGrounded;
        private float _movementSpeed;
        private float _jumpSpeed;
        private float _climbSpeed;
        [SerializeField]
        private PatrolWalker _patrolComponent;

        [SerializeField]
        private Follower _followComponent;

        [SerializeField]
        private MultiGoalWalker _goalComponent;


        [Header("Graphics")]
        [SerializeField]
        private bool _isDrawingLookingLeft = false;

        [SerializeField]
        private Transform _gfx;
        [SerializeField]
        internal Animator _animator;
        private float _frozenCoef = 1;

        private void OnEnable()
        {
            _aiStateChannel.OnStateEnter += OnStateEnter;
            _aiStateChannel.OnStateExit += OnStateExit;

            _navAgent.OnReachedGoal += Agent_OnReachedGoal;
            _navAgent.OnStartLinkTraversal += Agent_StartLinkTraversalEvent;
            _navAgent.OnStartSegmentTraversal += Agent_OnStartSegmentTraversal;

            _movementSpeed = _tbMvt.movementSpeed;
            _jumpSpeed = _tbMvt.jumpSpeed;
            _climbSpeed = _tbMvt.climbSpeed;

            //_gfx.rotation = new Quaternion(0, 0, 0, 0);
        }

        private void OnDisable()
        {
            _aiStateChannel.OnStateEnter -= OnStateEnter;
            _aiStateChannel.OnStateExit -= OnStateExit;

            _navAgent.OnReachedGoal -= Agent_OnReachedGoal;
            _navAgent.OnStartLinkTraversal -= Agent_StartLinkTraversalEvent;
            _navAgent.OnStartSegmentTraversal -= Agent_OnStartSegmentTraversal;
        }

        #region NavAgent


        internal bool IsAgentStuck(Vector2 _lastPosition)
        {
            //todo handle "Cannot reach" scenario?
            //"Cannot reach" => When the target is in range but cannot jump to attack
            return Vector2.Distance(_navAgent.Position, _lastPosition) <= 0.03f;
        }

        internal Vector2 GetAgentPosition()
        {
            return _navAgent.Position;
        }

        public void StopMoving()
        {
            try
            {
                _navAgent.Stop();
            }
            catch (NullReferenceException e)
            {
                _navAgent.ForceStop();
                Console.WriteLine("Error stoping : " + e.Message);
            }
        }

        internal void SetSlowRatio(float frozenCoef)
        {
            _frozenCoef = frozenCoef;
            SetSpeed();
        }

        internal void SetSpeed()
        {

            var combatCoef = _isInCombat ? RunningFactor : 0;
            var speedCoef = _frozenCoef + combatCoef;

            _tbMvt.movementSpeed = _movementSpeed * speedCoef;
            _tbMvt.climbSpeed = _climbSpeed * speedCoef;
            _tbMvt.jumpSpeed = _jumpSpeed / 2 + speedCoef / 2 * speedCoef;
        }

        public void Patrol()
        {
            if (!_patrolComponent.enabled)
            {
                _patrolComponent.enabled = true;
                _patrolComponent.RefreshPath();
            }
            else
            {
                _patrolComponent.RefreshPath();
            }
        }

        public void MoveToClosestGoal()
        {
            if (!_goalComponent.enabled)
            {
                _goalComponent.enabled = true;
            }
            _goalComponent.MoveToClosestGoal();
        }

        public void MoveToGoal(Transform goal)
        {
            if (!_goalComponent.enabled)
            {
                _goalComponent.enabled = true;
            }
            var goals = new Transform[] { goal };
            _goalComponent.MoveToClosestGoal(goals);
        }

        public void StopPatrol()
        {
            //StopMoving();
            _patrolComponent.enabled = false;
        }

        public void Follow(Transform focus)
        {
            if (!_followComponent.enabled)
            {
                _followComponent.target = focus;
                _followComponent.enabled = true;
            }
        }

        internal void StopFollow()
        {
            //StopMoving();
            _followComponent.enabled = false;
        }

        private void Agent_OnReachedGoal(NavAgent obj)
        {
            //_animator.SetFloat("Speed", 0);
            //Debug.Log("Goal reached");
        }

        private void Agent_OnStartSegmentTraversal(NavAgent agent)
        {
            _isGrounded = true;

            if (_currentState.IsMovingState())
            {
                HandleWalkAnimation();
            }
            var isGoingLeft = IsGoingLeftFromSegment(agent);
            HandleLookingSide(isGoingLeft);
        }

        private void HandleWalkAnimation()
        {
            PlayAnimation(_isInCombat ? "WalkCombat": "Walk");
        }

        private void Agent_StartLinkTraversalEvent(NavAgent agent)
        {
            var isGoingLeft = IsGoingLeftFromLink(agent);
            HandleLookingSide(isGoingLeft);

            if(agent.CurrentPathSegment.link == null)
            {
                return;
            }

            _isGrounded = false;
            switch (agent.CurrentPathSegment.link.LinkTypeName)
            {
                case "jump":
                    PlayAnimation("Jump");
                    break;
                case "fall":
                    PlayAnimation("Fall");
                    break;
                default:
                    NoHandle(agent);
                    break;
            }
        }

        private void NoHandle(NavAgent agent)
        {
            Debug.LogWarning("Behavior not handle");
        }
        #endregion //NavAgent

        #region Events


        private void OnStateEnter(EnemyStateEvent stateEvt)
        {
            if (name != stateEvt.EnemyId)
            {
                return;
            }
            _currentState = stateEvt.Type;
            _isInCombat = stateEvt.Type.IsCombatState();
            SetSpeed();
        }

        private void OnStateExit(EnemyStateEvent stateEvt)
        {
            if (name != stateEvt.EnemyId)
            {
                return;
            }

            _isInCombat = false;
        }

        #endregion //Events

        #region LookingSide

        private bool IsGoingLeftFromSegment(NavAgent agent)
        {
            var pos = Geometry.ProjectPointOnLine(agent.Position, agent.CurrentPathSegment.Point, agent.CurrentPathSegment.Tangent);
            var goal = Geometry.ProjectPointOnLine(agent.PathSubGoal, agent.CurrentPathSegment.Point, agent.CurrentPathSegment.Tangent);
            Vector2 direction = goal - pos;

            return IsVectorLeft(direction);
        }

        private bool IsGoingLeftFromLink(NavAgent agent)
        {
            Vector2 delta = agent.PathSubGoal - agent.CurrentPathSegment.LinkStart;
            var direction = delta / delta.magnitude;
            return IsVectorLeft(direction);
        }
        internal bool IsAgentLookingLeft()
        {
            return !GfxHasRotation() && _isDrawingLookingLeft || GfxHasRotation() && !_isDrawingLookingLeft;
        }

        private bool GfxHasRotation()
        {
            return _gfx.rotation.y != 0;
        }

        private void HandleLookingSide(bool isGoingLeft)
        {
            
            if (_isDrawingLookingLeft)
            {
                var angle = isGoingLeft ? 0 : 180;
                _gfx.transform.rotation = Quaternion.Euler(0, angle, 0);
                return;
            }

            if (!_isDrawingLookingLeft)
            {
                var angle = isGoingLeft ? 180 : 0;
                _gfx.transform.rotation = Quaternion.Euler(0, angle, 0);
                return;
            }
        }

        public void LookOtherSide()
        {
            var angle = _gfx.transform.rotation.y == 0 ? 180 : 0;
            _gfx.transform.rotation = Quaternion.Euler(0, angle, 0);
        }


        internal void HandleLookingSide(Vector2 origin, Transform targetTransform)
        {
            if(targetTransform == null)
            {
                return;
            }
            Vector2 target = targetTransform.position;

            var direction = (target - origin).normalized;

            HandleLookingSide(IsVectorLeft(direction));
        }

        private bool IsVectorLeft(Vector2 direction)
        {
            return direction.x <= 0;
        }
        #endregion //LookingSide

        #region Animator
        internal void PlayAnimation(string animName)
        {
            //Debug.Log(animName);
            _animator.Play(animName);
        }

        internal void Push(float v)
        {
            transform.position += new Vector3(IsAgentLookingLeft()? -v : v, 0, 0);
        }

        internal void TakeRepulse(Vector2 repulseMagnitude)
        {
            transform.position += new Vector3(repulseMagnitude.x, repulseMagnitude.y, 0);
        }

        internal bool IsGrounded()
        {
            return _isGrounded;
        }
        #endregion //Animator
    }
}
