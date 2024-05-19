using UnityEngine;

namespace Assets.Data.Enemy
{
    public class EnemyDamageTakenEvent
    {
        public Vector2 position;
        public int Damage;
        public HealthEffectorType Type;
        public bool IsCriticalStrike;
    }
}
