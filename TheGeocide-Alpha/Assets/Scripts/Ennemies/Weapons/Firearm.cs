using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets.Scripts.Ennemies.Weapons
{
    public class Firearm : MonoBehaviour, IEnemyRangedWeapon
    {
        public int BulletDammage = 20;
        [SerializeField]
        private FirearmFlameFX _flamePrefab;
        private FirearmFlameFX _currentFlame;
        [SerializeField]
        private AudioSource _audio;
        [SerializeField]
        private Projectile _bulletPrefab;
        [SerializeField]
        private Transform _StockPoint;
        [SerializeField]
        private Transform _CanonPoint;
        [SerializeField]
        private Rig _aimingRigLayer;
        [SerializeField]
        private Transform _aimingPoint;

        private Transform _aimingTarget;
        private float _aimSpeed = 0.3f;
        private bool _isAimingLocked;

        // Start is called before the first frame update
        void Start()
        {
            _currentFlame = Instantiate(_flamePrefab, _CanonPoint);
        }

        // Update is called once per frame
        void Update()
        {
            HandleAiming();
        }

        private void HandleAiming()
        {
            if (_isAimingLocked)
            {
                return;
            }

            if (_aimingTarget != null)
            {
                _aimingPoint.position = new Vector2(_aimingTarget.position.x , _aimingTarget.position.y + 0.7f);
                _aimingRigLayer.weight += Time.deltaTime / _aimSpeed;
            }
            else
            {
                _aimingPoint.position = Vector2.zero;
                _aimingRigLayer.weight -= Time.deltaTime / _aimSpeed;
            }
        }

        public void SetAiming(Transform target)
        {
            _aimingTarget = target;
        }

        public void SetAimingLock(bool isShooting)
        {
            _isAimingLocked = isShooting;
        }

        public void Shoot(bool isChanneling = false)
        {
            _audio.Play();
            _currentFlame.Init();
            _HandleProjectile();
        }

        private void _HandleProjectile()
        {
            var prefab = Instantiate(_bulletPrefab, _CanonPoint.position, _CanonPoint.rotation);

            Projectile projectile = prefab.GetComponent<Projectile>();

            //Override projectile damage to ensure SO values are used
            projectile.dommage = BulletDammage;

            projectile.Fire(1);
        }

        public void SpecialShoot(bool isChanneling = false)
        {
            throw new System.NotImplementedException();
        }
    }
}
