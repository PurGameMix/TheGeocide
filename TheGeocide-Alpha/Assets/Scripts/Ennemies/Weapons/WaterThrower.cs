using Assets.Scripts.Spell.Entity;
using System;
using UnityEngine;
namespace Assets.Scripts.Ennemies.Weapons
{
    public class WaterThrower : MonoBehaviour, IEnemyRangedWeapon
    {   

        [SerializeField]
        private ChannelSpell _basicAttackFX;
        public int DamagePerTick = 5;
        public int TickPerSecond = 2;
        private ChannelSpell _currentBasicWaterSpell;

        [SerializeField]
        private InstantSpell _repulseAttackFX;
        [SerializeField]
        private Transform _RepulseOrigin;
        public float BigRepulseForce = 100;
        public int RepulseDamage = 20;

        [SerializeField]
        private GPTankSpecialAttackSpell _specialAttackSpell;
        private GPTankSpecialAttackSpell _currentSpecialWaterSpell;

        [SerializeField]
        private AudioController _audioController;
        [SerializeField]
        private Transform _StockPoint;
        [SerializeField]
        private Transform _CanonPoint;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Shoot(bool isChanneling)
        {
            if (isChanneling)
            {
                if (_currentBasicWaterSpell == null)
                {
                    _currentBasicWaterSpell = Instantiate(_basicAttackFX, _CanonPoint);
                    _currentBasicWaterSpell.Init(DamagePerTick, TickPerSecond, HealthEffectorType.enemy);
                }
                //Debug.Log(DateTime.UtcNow + "Basic start");
                _currentBasicWaterSpell.StartChannel();
                return;
            }
            //Debug.Log(DateTime.UtcNow + "Basic stop");
            _currentBasicWaterSpell.StopChannel();
        }

        public void SpecialShoot(bool isChanneling)
        {
            if (isChanneling)
            {
                if (_currentSpecialWaterSpell == null)
                {
                    _currentSpecialWaterSpell = Instantiate(_specialAttackSpell, _CanonPoint);
                    _currentSpecialWaterSpell.Init(DamagePerTick, TickPerSecond, HealthEffectorType.enemy);
                }

                _currentSpecialWaterSpell.StartChannel();
                return;
            }

            _currentSpecialWaterSpell.StopChannel();
        }

        public void RepulseShoot()
        {
            var repulseSpell = Instantiate(_repulseAttackFX, _CanonPoint.position, Quaternion.identity);
            repulseSpell.Init(new EnemySpellInfos() {
                Damage = RepulseDamage,
                Effector = HealthEffectorType.enemy,
                RepulseForce = BigRepulseForce,
                RepulseOrigin = _RepulseOrigin
            });
        }
    }
}
