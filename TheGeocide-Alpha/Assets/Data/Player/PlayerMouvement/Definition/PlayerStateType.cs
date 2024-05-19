using System.Collections.Generic;

namespace Assets.Data.PlayerMouvement.Definition
{
    public enum PlayerStateType
    {
        AttackRanged_Cast,
        AttackRanged_Release,
        AttackMelee_Cast,
        AttackMelee_Release,
        AttackUppercut,
        AttackLanding,
        AttackLanded,
        AttackUltimate,
        Climb,
        Crouch,
        Dash,
        Dead,
        EdgeClimb,
        FallDead,
        FallHurt,
        Hurt,
        Idle,
        IdleClimb,
        IdleCrouch,
        IdleRope,
        IdleSwim,
        InAir,
        Jump,
        OnZipline,
        Pushing,
        Rope,
        RopeSwing,
        Run,
        SlopeSlide,
        Stun,
        Swim,
        WallJump,
        WallSlide,
    }

    public static class PlayerStateTypeExtensions
    {

        private static List<PlayerStateType> _isMeleeState = new List<PlayerStateType>()
        {
            PlayerStateType.AttackMelee_Cast,
            PlayerStateType.AttackMelee_Release,
        };

        private static List<PlayerStateType> _isRangedState = new List<PlayerStateType>()
        {
            PlayerStateType.AttackRanged_Cast,
            PlayerStateType.AttackRanged_Release
        };

        private static List<PlayerStateType> _isOverrideState = new List<PlayerStateType>()
        {
            PlayerStateType.AttackRanged_Cast,
            PlayerStateType.AttackRanged_Release,
            PlayerStateType.AttackMelee_Cast,
            PlayerStateType.AttackMelee_Release,
            PlayerStateType.Hurt
        };

        private static List<PlayerStateType> _canBeOverrideLegsState = new List<PlayerStateType>()
        {
            PlayerStateType.Idle,
        };


        public static bool IsMeleeCombatState(this PlayerStateType pst)
        {
            return _isMeleeState.Contains(pst);
        }

        public static bool IsRangedCombatState(this PlayerStateType pst)
        {
            return _isRangedState.Contains(pst);
        }


        public static bool IsStandardState(this PlayerStateType pst)
        {
            return !_isOverrideState.Contains(pst);
        }

        public static bool IsBodyPlayed(this PlayerStateType pst, PlayerStateType previousState)
        {
            if (_isOverrideState.Contains(pst))
            {
                return true;
            }

            return !_isOverrideState.Contains(previousState);
        }

        public static bool IsLegPlayed(this PlayerStateType pst, PlayerStateType previousState)
        {
            if (!_isOverrideState.Contains(pst))
            {
                return true;
            }

            return _canBeOverrideLegsState.Contains(previousState);
        }

        private static List<PlayerStateType> _canfightStateList = new List<PlayerStateType>() {
            PlayerStateType.Idle,
            PlayerStateType.InAir,
            PlayerStateType.Jump,
            PlayerStateType.OnZipline,
            PlayerStateType.Run,
            PlayerStateType.WallJump,
            PlayerStateType.SlopeSlide,
            PlayerStateType.Swim,
            PlayerStateType.IdleSwim
        };

        public static bool IsAttackPossible(this PlayerStateType pst)
        {
            return _canfightStateList.Contains(pst);
        }

        private static Dictionary<PlayerStateType,string> _audioDico = new Dictionary<PlayerStateType, string>
        {
            {PlayerStateType.AttackMelee_Cast,  "Player_Melee_Cast"},
            {PlayerStateType.AttackMelee_Release,  "Player_Melee_Release"},
            {PlayerStateType.AttackRanged_Cast,  "Player_Ranged_Cast"},
            {PlayerStateType.AttackRanged_Release, "Player_Ranged_Release"}
        };

        public static string GetAudioKey(this PlayerStateType pst)
        {
            if (_audioDico.ContainsKey(pst))
            {
                return _audioDico[pst];
            }

            return $"no audio found for {pst}";
        }


        private static List<PlayerStateType> _cannotTurnStateList = new List<PlayerStateType>() {
            PlayerStateType.Dash,
            PlayerStateType.OnZipline,
            PlayerStateType.Stun,
            PlayerStateType.SlopeSlide,
            PlayerStateType.EdgeClimb,
            PlayerStateType.Rope
        };
        public static bool IsTurnForbidden(this PlayerStateType pst)
        {
            return _cannotTurnStateList.Contains(pst);
        }
    }
}