using Assets.Scripts.Player.Mouvement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets.Scripts.Player.Animations.Rigging
{
    public class PlayerRopePhysicsController : MonoBehaviour
    {

        [SerializeField]
        private DistanceJoint2D _joint2D;

        [SerializeField]
        private PlayerFacingController _facingController;

        [SerializeField]
        private Rig _ropeRigLayer;

        //[SerializeField]
        //private TwoBoneIKConstraint _backArm;

        //[SerializeField]
        //private TwoBoneIKConstraint _backLeg;

        [SerializeField]
        private TwoBoneIKConstraint _frontArm;

        [SerializeField]
        private TwoBoneIKConstraint _frontLeg;

        private float _rigMaxSpeed = 10;
        private float _rigNormalSpeed = 0.5f;

        private Vector2 _defaultAnchor;

        private bool _isOnRope;
        private bool _hasValidLinkToCatch;
        private bool _isRopeClimbing;

        private int _maxUpLink = 4;
        private int _minDownLink = 4;
        private Dictionary<string, Transform> _ropeSolverDico = new Dictionary<string, Transform>();


        private Rigidbody2D currentRopeLink;
        private Rigidbody2D[] currentRope;
        private int currentRopeLinkIndex;


        private RopeLayerRigConfig _currentConfig = RopeLayerRigConfig.Disable;

        // Start is called before the first frame update
        void Start()
        {
            InputHandler.instance.OnDeplacement += OnDeplacement;
            _defaultAnchor = _joint2D.anchor;
            GetRopeSolvers();
        }

        // Update is called once per frame
        void Update()
        {
            if(_currentConfig == RopeLayerRigConfig.Disable && _ropeRigLayer.weight > 0)
            {
                _ropeRigLayer.weight -= Time.deltaTime * _rigMaxSpeed;
                return;
            }

            if (_currentConfig == RopeLayerRigConfig.Half && _ropeRigLayer.weight > 0.2f)
            {
                _ropeRigLayer.weight -= Time.deltaTime * _rigNormalSpeed;
                return;
            }

            if (_currentConfig == RopeLayerRigConfig.Half && _ropeRigLayer.weight < 0.1f)
            {
                _ropeRigLayer.weight += Time.deltaTime * _rigNormalSpeed;
                return;
            }

            if (_currentConfig == RopeLayerRigConfig.Full && (_ropeRigLayer.weight < 1 || _frontArm.weight <1))
            {
                _ropeRigLayer.weight += Time.deltaTime * _rigMaxSpeed;
                _frontArm.weight += Time.deltaTime * _rigMaxSpeed;
                _frontLeg.weight += Time.deltaTime * _rigMaxSpeed;
                return;
            }

            if (_currentConfig == RopeLayerRigConfig.Front && (_ropeRigLayer.weight < 1 || _frontArm.weight > 0))
            {
                _ropeRigLayer.weight += Time.deltaTime * _rigMaxSpeed;
                _frontArm.weight -= Time.deltaTime * _rigMaxSpeed;
                _frontLeg.weight -= Time.deltaTime * _rigMaxSpeed;
                return;
            }
        }

        public void SetConfig(RopeLayerRigConfig config)
        {
            _currentConfig = config;
        }

        private void OnDeplacement(InputHandler.InputArgs obj)
        {
            if (_joint2D.connectedBody == null)
            {
                return;
            }

            if (InputHandler.instance.MoveInput.x < 0 && _joint2D.anchor.x < 0)
            {
                _joint2D.anchor = new Vector2(-_joint2D.anchor.x, _joint2D.anchor.y);
            }

            if (InputHandler.instance.MoveInput.x > 0 && _joint2D.anchor.x > 0)
            {
                _joint2D.anchor = new Vector2(-_joint2D.anchor.x, _joint2D.anchor.y);
            }

            if (InputHandler.instance.MoveInput.y != 0)
            {
                CheckDirectionToFaceOnRope();
            }
        }

        internal void IdleRopeClimb()
        {
            if(_joint2D.anchor.y == _defaultAnchor.y)
            {
                return;
            }
            _joint2D.anchor = new Vector2(_joint2D.anchor.x, _defaultAnchor.y);
        }

        internal void RopeClimb()
        {          
            var ecart = 0.01f; //todo Improve this part to match animation if animation updated

            if (!_isRopeClimbing)
            {
                _joint2D.anchor = new Vector2(_joint2D.anchor.x, _defaultAnchor.y);
                return;
            }

            var isClimbingUp = InputHandler.instance.MoveInput.y > 0;
            var currentY = _joint2D.anchor.y;
            currentY = isClimbingUp ? currentY - ecart : currentY + ecart;


            _joint2D.anchor = new Vector2(_joint2D.anchor.x, currentY);
        }

        public void RopeClimbBegin()
        {
            _isRopeClimbing = true;
        }

        internal void CancelRopeClimbing()
        {
            _isRopeClimbing = false;
        }

        public void RopeClimbLink()
        {
            _isRopeClimbing = false;
            var isClimbingUp = InputHandler.instance.MoveInput.y > 0;
            var nextLink = GetNextLink(isClimbingUp);
            if (nextLink == null)
            {
                return;
            }


            currentRopeLinkIndex = isClimbingUp ? currentRopeLinkIndex - 1 : currentRopeLinkIndex + 1;
            _joint2D.connectedBody = nextLink;

            SetSolversOnCurrentRope();
        }

        private Rigidbody2D GetNextLink(bool isClimbingUp)
        {
            if (currentRope == null)
            {
                return null;
            }

            if (!isClimbingUp && currentRopeLinkIndex == currentRope.Length - _minDownLink)
            {
                return null;
            }

            if (isClimbingUp && currentRopeLinkIndex == _maxUpLink)
            {
                return null;
            }

            var nextLinkIndex = isClimbingUp ? currentRopeLinkIndex - 1 : currentRopeLinkIndex + 1;
            return currentRope[nextLinkIndex];
        }

        public void SetSolversOnCurrentRope()
        {
            if (currentRopeLinkIndex - 3 < 0
                || currentRopeLinkIndex + 3 > currentRope.Length
                )
            {
                Debug.Log("Error");
            }

            var backArmLink = currentRope[currentRopeLinkIndex - 3];
            var frontArmLink = currentRope[currentRopeLinkIndex - 2];
            var feetLinkLink = currentRope[currentRopeLinkIndex + 3];


            SetSolver(backArmLink.transform, _ropeSolverDico["BackArmRopeSolver_ref"]);
            SetSolver(frontArmLink.transform, _ropeSolverDico["FrontArmRopeSolver_ref"]);
            SetSolver(feetLinkLink.transform, _ropeSolverDico["BackLegRopeSolver_ref"]);
            SetSolver(feetLinkLink.transform, _ropeSolverDico["FrontLegRopeSolver_ref"]);
        }

        private void SetSolver(Transform parentTransfrom, Transform solver)
        {
            solver.parent = parentTransfrom;
            solver.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void GetRopeSolvers()
        {
            var entity = GameObject.Find("RopeSolvers");
            if (entity == null)
            {
                Debug.LogError("RopeSolvers are missing in the scene");
            }

            for (int i = 0; i < entity.transform.childCount; i++)
            {

                GameObject go = entity.transform.GetChild(i).gameObject;
                //go.GetComponent<SpriteRenderer>().enabled = false;
                _ropeSolverDico.Add(go.name, go.transform);
            }
        }

        internal void CheckHangingPosition()
        {
            StartCoroutine(WaitForceToCheckRopePosition());
        }
        private IEnumerator WaitForceToCheckRopePosition()
        {
            yield return new WaitForSeconds(0.1f);
            CheckDirectionToFaceOnRope();
        }

        public void CheckDirectionToFaceOnRope()
        {
            if (_joint2D.connectedBody == null)
            {
                return;
            }
            var direction = _joint2D.connectedBody.transform.position.x - _joint2D.transform.position.x;

            if (direction >= 0)
            {
                _facingController.CheckDirectionToFace(true);
                if (_joint2D.anchor.x < 0)
                {
                    _joint2D.anchor = new Vector2(-_joint2D.anchor.x, _joint2D.anchor.y);
                }
            }

            if (direction < 0)
            {
                _facingController.CheckDirectionToFace(false);
                if (_joint2D.anchor.x > 0)
                {
                    _joint2D.anchor = new Vector2(-_joint2D.anchor.x, _joint2D.anchor.y);
                }
            }
        }

        public void AttachPlayerToRope()
        {
            _joint2D.connectedBody = currentRopeLink;
            _joint2D.enabled = true;
            _isOnRope = true;

            CheckHangingPosition();
            Debug.Log($"{DateTime.Now}: attachRope");
        }
        public void DetachPlayerToRope()
        {
            _isOnRope = false;
            _hasValidLinkToCatch = false;
            currentRopeLink = null;
            currentRope = null;
            currentRopeLinkIndex = -1;
            _joint2D.connectedBody = null;
            _joint2D.enabled = false;
            Debug.Log($"{DateTime.Now}: dettachRope");
        }


        internal bool CanCatchRope()
        {
            return _hasValidLinkToCatch;
        }

        internal bool IsOnRope()
        {
            return _isOnRope;
        }

        internal void RegisterValidRopeLink(Rigidbody2D link)
        {
            var rope = GetRope(link);
            var index = GetRopeIndex(rope, link);

            if (index >= _maxUpLink && index < rope.Length - _minDownLink)
            {
                currentRopeLink = link;
                currentRope = rope;
                currentRopeLinkIndex = index;
                _hasValidLinkToCatch = true;
            }
            else
            {
                _hasValidLinkToCatch = false;
            }
        }

        private int GetRopeIndex(Rigidbody2D[] currentRope, Rigidbody2D link)
        {
            var i = 0;
            foreach (var rb in currentRope)
            {

                if (rb == link)
                {
                    return i;
                }
                i++;
            }

            throw new Exception("RopeLinkNotFound");
        }

        private Rigidbody2D[] GetRope(Rigidbody2D currentRopeLink)
        {
            return currentRopeLink.transform.parent.GetComponentsInChildren<Rigidbody2D>();
        }
    }
}