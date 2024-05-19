using Assets.Data.PlayerMouvement.Definition;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    [SerializeField]
    private PlayerStateChannel _playerStateChannel;
    [SerializeField]
    private Animator _playerAnimator;
    [SerializeField]
    private AudioController _playerAudioController;

    private Dictionary<PlayerStateType, string> _animationClipMap;
    private Dictionary<PlayerStateType, string> _stateSoundMap;

    private PlayerStateEvent _previousStandardState;
    private PlayerStateEvent _currentState;

    private AimingStateEvent _aimingState = new AimingStateEvent();
    // Start is called before the first frame update
    void Awake()
    {
        _playerStateChannel.OnMouvementStateEnter += OnMouvementStateEnter;
        _playerStateChannel.OnMouvementStateExit += OnMouvementStateExit;
        _playerStateChannel.OnAimingUpdate += OnAimingUpdate;

        _animationClipMap = new Dictionary<PlayerStateType, string>()
        {
            {PlayerStateType.AttackMelee_Cast, "Player_Melee_Cast"},
            {PlayerStateType.AttackMelee_Release, "Player_Melee_Release"},
            {PlayerStateType.AttackRanged_Cast, "Player_Ranged_Cast"},
            {PlayerStateType.AttackRanged_Release, "Player_Ranged_Release"},
            {PlayerStateType.AttackLanding, "Player_Landing"},
            {PlayerStateType.AttackLanded, "Player_Landed"},
            {PlayerStateType.AttackUppercut, "Player_Uppercut"},
            {PlayerStateType.AttackUltimate, "Player_Ultimate"},
            {PlayerStateType.Idle, "Idle"},
            {PlayerStateType.Jump, "Jump"},
            {PlayerStateType.Dash, "Dash"},
            {PlayerStateType.Run, "Walk"},
            {PlayerStateType.InAir, "InAir"},
            {PlayerStateType.Climb, "Climb"},
            {PlayerStateType.IdleClimb, "IdleClimb"},
            {PlayerStateType.WallJump, "WallJump"},
            {PlayerStateType.WallSlide, "WallSlide"},
            {PlayerStateType.Rope, "RopeClimb"},
            {PlayerStateType.RopeSwing, "RopeSwing"},
            {PlayerStateType.IdleRope, "IdleRope"},
            {PlayerStateType.OnZipline, "Zipline"},
            {PlayerStateType.Hurt, "Hurt"},
            {PlayerStateType.Dead, "Die"},
            {PlayerStateType.FallHurt, "FallHurt"},
            {PlayerStateType.FallDead, "FallDie"},
            {PlayerStateType.Pushing, "Push"},
            {PlayerStateType.Stun, "Stun"},
            {PlayerStateType.Crouch, "Crouch"},
            {PlayerStateType.IdleCrouch, "IdleCrouch"},
            {PlayerStateType.SlopeSlide, "SlopeSlide"},
            {PlayerStateType.IdleSwim, "IdleSwim"},
            {PlayerStateType.Swim, "Swim"},
            {PlayerStateType.EdgeClimb, "EdgeClimb"},
        };

        _stateSoundMap = new Dictionary<PlayerStateType, string>()
        {
            {PlayerStateType.Jump, "Jump"},
            {PlayerStateType.Dash, "Dash"},
            {PlayerStateType.Hurt, "Hurt"},
            {PlayerStateType.Dead, "Dead"},
            {PlayerStateType.FallHurt, "FallHurt"},
            {PlayerStateType.FallDead, "FallDie"},
            {PlayerStateType.Pushing, "Push"},
            {PlayerStateType.Stun, "Stun"},
            {PlayerStateType.SlopeSlide, "SlopeSlide"},
        };

        _currentState = _previousStandardState = new PlayerStateEvent(PlayerStateType.Idle);
    }

    private void Start()
    {
        InputHandler.instance.OnDeplacement += OnDeplacement;
    }

    void OnDestroy()
    {
        _playerStateChannel.OnMouvementStateEnter -= OnMouvementStateEnter;
        _playerStateChannel.OnMouvementStateExit -= OnMouvementStateExit;
        InputHandler.instance.OnDeplacement -= OnDeplacement;
    }

    private void OnDeplacement(InputHandler.InputArgs obj)
    {
       if(_currentState.Type != PlayerStateType.Run)
        {
            return;
        }

       //Recalculation of the animation if same state
        EnterRunState(_currentState);
    }

    private void OnMouvementStateEnter(PlayerStateEvent mvtEvt)
    {
        //todo passer en visitor?
        switch (mvtEvt.Type)
        {
            case PlayerStateType.Run: EnterRunState(mvtEvt); break;
            case PlayerStateType.Jump: EnterPlaySound(mvtEvt); break;
            case PlayerStateType.Dash: EnterPlaySound(mvtEvt); break;
            case PlayerStateType.Hurt:EnterPlaySound(mvtEvt);break;
            case PlayerStateType.Dead: EnterPlaySound(mvtEvt); break;
            case PlayerStateType.FallHurt: EnterPlaySound(mvtEvt); break;
            case PlayerStateType.FallDead: EnterPlaySound(mvtEvt); break;
            case PlayerStateType.Pushing: EnterPlaySound(mvtEvt); break;
            case PlayerStateType.Stun: EnterPlaySound(mvtEvt); break;
            case PlayerStateType.SlopeSlide: EnterPlaySound(mvtEvt); break;
            default: EnterGeneric(mvtEvt); break;
        }
    }

    private void OnMouvementStateExit(PlayerStateEvent mvtEvt)
    {
        //todo passer en visitor?
        switch (mvtEvt.Type)
        {
            case PlayerStateType.AttackMelee_Cast: ExitAttackMelee_Cast(mvtEvt); break;
            case PlayerStateType.AttackMelee_Release: ExitAttackMelee_Release(mvtEvt); break;
            case PlayerStateType.AttackRanged_Cast: ExitAttackRanged_Cast(mvtEvt); break;
            case PlayerStateType.AttackRanged_Release: ExitAttackRanged_Release(mvtEvt); break;
            case PlayerStateType.Hurt: ExitHurt(mvtEvt); break;
            case PlayerStateType.Pushing: ExitStopSound(mvtEvt); break;
            case PlayerStateType.Stun: ExitStopSound(mvtEvt); break;
            case PlayerStateType.SlopeSlide: ExitStopSound(mvtEvt); break;
            default: ExitGeneric(mvtEvt); break;
        }
    }


    private void OnAimingUpdate(AimingStateEvent tdEvt)
    {
        _aimingState = tdEvt;
    }
    #region Common
    private bool IsValid()
    {
        return _playerAnimator != null && _playerAudioController != null;
    }

    private void HandleAnimation(PlayerStateEvent mvtEvt, string clipName)
    {
        if (mvtEvt.Type.IsBodyPlayed(_currentState.Type))
        {
            //Debug.Log($"{_animationClipMap[mvtEvt.Type]}_Body");
            _playerAnimator.Play($"{clipName}_Body");
        }

        if (mvtEvt.Type.IsLegPlayed(_currentState.Type))
        {
            _playerAnimator.Play($"{clipName}_Legs");
        }

        if (mvtEvt.Type.IsStandardState())
        {           
            _previousStandardState = mvtEvt;
        }
        
        _currentState = mvtEvt;
    }

    private void PlayPreviousStandardAnimation()
    {
        _playerAnimator.Play($"{_animationClipMap[_previousStandardState.Type]}_Body");
        _playerAnimator.Play($"{_animationClipMap[_previousStandardState.Type]}_Legs");

        _currentState = _previousStandardState;
    }

    private void StopAnimation(PlayerStateEvent mvtEvt)
    {
        //_previousStandardAnimation = null;
    }
    #endregion //Comon

    #region Enter Method

    private void EnterGeneric(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }

        HandleAnimation(mvtEvt, _animationClipMap[mvtEvt.Type]);
    }

    private void EnterPlaySound(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }
        _playerAudioController.Play(_stateSoundMap[mvtEvt.Type]);
        HandleAnimation(mvtEvt, _animationClipMap[mvtEvt.Type]);
    }

    private void EnterRunState(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }

        if (!_aimingState.IsAiming)
        {
            HandleAnimation(mvtEvt, _animationClipMap[mvtEvt.Type]);
            return;
        }

        var aimOppositeToMovementDirection = Mathf.Sign(InputHandler.instance.MoveInput.x) != Mathf.Sign(_aimingState.Direction.x);
        if (aimOppositeToMovementDirection)
        {
            HandleAnimation(mvtEvt, "WalkBackward");
            return;
        }

        HandleAnimation(mvtEvt, _animationClipMap[mvtEvt.Type]);
    }
    #endregion //Enter Methods

    #region Exit Methods
    private void ExitGeneric(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }
        StopAnimation(mvtEvt);
    }

    private void ExitAttackMelee_Cast(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }
        StopAnimation(mvtEvt);
        PlayPreviousStandardAnimation();

    }

    private void ExitAttackMelee_Release(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }
        StopAnimation(mvtEvt);
        PlayPreviousStandardAnimation();
    }

    private void ExitAttackRanged_Cast(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }
        StopAnimation(mvtEvt);
        PlayPreviousStandardAnimation();
    }

    private void ExitAttackRanged_Release(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }
        StopAnimation(mvtEvt);
        PlayPreviousStandardAnimation();
    }

    private void ExitHurt(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }
        StopAnimation(mvtEvt);
        PlayPreviousStandardAnimation();
    }

    private void ExitStopSound(PlayerStateEvent mvtEvt)
    {
        if (!IsValid())
        {
            return;
        }
        StopAnimation(mvtEvt);
        _playerAudioController.Stop(_stateSoundMap[mvtEvt.Type]);
    }
    #endregion //Exit methods
}
