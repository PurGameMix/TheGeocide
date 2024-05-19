
namespace Assets.Scripts.Ennemies.Weapons
{
    public interface IEnemyRangedWeapon
    {
        /// <summary>
        /// The default shoot of a weapon
        /// </summary>
        /// <param name="isChanneling">Use for determine if a chaneling weapon is shooting or not</param>
        public void Shoot(bool isChanneling = false);

        /// <summary>
        /// A secondary mode on the weapon allowing powerfull shot
        /// </summary>
        /// <param name="isChanneling">Use for determine if a chaneling weapon is shooting or not</param>
        public void SpecialShoot(bool isChanneling = false);
    }
}
