using Assets.Scripts.Ennemies.Weapons;
using Assets.Scripts.PathBerserker2dExtentions;
using System;
using UnityEngine;

namespace Assets.Scripts.Timelines
{
    public class EnemyIATimelineDirector : MonoBehaviour
    {
        [SerializeField]
        private EnemyIAMouvement _enemyMvt;

        [SerializeField]
        private WaterThrower _enemyWeapon;

        public void MoveToClosestGoal()
        {
            _enemyMvt.MoveToClosestGoal();
        }

        public void MoveToGoal(Transform goal)
        {
            _enemyMvt.MoveToGoal(goal);
        }

        public void LookOtherSide()
        {
            _enemyMvt.LookOtherSide();
        }

        internal void TPEnemyToPosition(Vector2 teleportPosition)
        {
            _enemyMvt.gameObject.transform.position = teleportPosition;
        }

        internal Vector2 GetPosition()
        {
            return _enemyMvt.gameObject.transform.position;
        }

        internal void ShotWeapon(bool isOn)
        {
            _enemyWeapon.Shoot(isOn);
        }

        internal void Disable()
        {
            _enemyMvt.gameObject.SetActive(false);
        }
    }
}
