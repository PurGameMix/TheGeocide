using System.Collections.Generic;

namespace Assets.Data.Enemy.Definition
{
    public enum EnemyStateType
    {
        ActionAlarm,
        ActionCheck,
        ActionSuprised,
        AttackBasic,
        AttackCharge,
        AttackContact,
        AttackIdle,
        Die,
        Flee,
        Frozen,
        Hurt,
        HurtStun,
        Idle,
        Ignite,
        MovePatrol,
        MoveToPlayer,
        MoveToCheck,
        Electrocuted
    }

    public static class EnemyStateTypeExtensions
    {

        private static List<EnemyStateType> _isMovingList = new List<EnemyStateType>(){
            EnemyStateType.MovePatrol,
            EnemyStateType.MoveToCheck,
            EnemyStateType.MoveToPlayer
            };

        public static bool IsMovingState(this EnemyStateType est)
        {
            return _isMovingList.Contains(est);
        }

        private static List<EnemyStateType> _isCombatList = new List<EnemyStateType>(){
            EnemyStateType.AttackCharge,
            EnemyStateType.AttackContact,
            EnemyStateType.AttackCharge,
            EnemyStateType.AttackIdle,
            EnemyStateType.MoveToCheck,
            EnemyStateType.MoveToPlayer
            };

        public static bool IsCombatState(this EnemyStateType est)
        {
            return _isCombatList.Contains(est);
        }

        private static List<EnemyStateType> _isAttackList = new List<EnemyStateType>(){
            EnemyStateType.AttackCharge,
            EnemyStateType.AttackContact,
            EnemyStateType.AttackCharge
            };

        public static bool IsAttackState(this EnemyStateType est)
        {
            return _isAttackList.Contains(est);
        }
    }
}