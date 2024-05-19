using Assets.Data.PlayerMouvement.Definition;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Player.Mouvement
{
   public class PlayerFacingController : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerGfx;
        [SerializeField]
        internal PlayerStateChannel _playerStateChannel;
        internal AimingStateEvent aimState = new AimingStateEvent();

        private bool _isFacingRight;
        private PlayerStateType _currentState;
        private void Awake()
        {
            _isFacingRight = true;
            _playerStateChannel.OnAimingUpdate += OnAimingUpdated;
            _playerStateChannel.OnMouvementStateEnter += OnMouvementStateEnter;

        }

        private void OnMouvementStateEnter(PlayerStateEvent mvtEvt)
        {
            _currentState = mvtEvt.Type;
        }

        private void OnDestroy()
        {
            _playerStateChannel.OnAimingUpdate -= OnAimingUpdated;
        }

        private void OnAimingUpdated(AimingStateEvent aimEvt)
        {
            aimState = aimEvt;
        }

        public void CheckDirectionToFace(bool wantToFaceRight)
        {
            if (aimState.IsAiming)
            {
                return;
            }

            if (wantToFaceRight != _isFacingRight)
            {
                Turn();
            }
        }

        private void Turn()
        {
            _playerGfx.Rotate(0, 180, 0);
            _isFacingRight = !_isFacingRight;
        }

        internal void FlipAim(float mouseOffset)
        {

            if (_isFacingRight && mouseOffset < 0)
            {
                Turn();
                _playerStateChannel.RaiseAimingUpdate(new AimingStateEvent()
                {
                    IsAiming = true,
                    Direction = new Vector2(mouseOffset, 0)
                });
            }

            if (!_isFacingRight && mouseOffset > 0)
            {
                Turn();
                _playerStateChannel.RaiseAimingUpdate(new AimingStateEvent()
                {
                    IsAiming = true,
                    Direction = new Vector2(mouseOffset, 0)
                });
            }
        }

        public bool IsFacingRight()
        {
            return _isFacingRight;
        }

        public bool IsAiming()
        {
            return aimState.IsAiming;
        }

        public Vector2 GetAimingDirection()
        {
            return aimState.Direction;
        }
    }
}
