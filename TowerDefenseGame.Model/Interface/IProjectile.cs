namespace TowerDefenseGame.Model
{
    /// <summary>
    /// Interface of the projectile
    /// </summary>
    public interface IProjectile
    {
        /// <summary>
        /// Damage caused by the projectile
        /// </summary>
        /// <param name="damage">Value of damage caused to the enemy</param>
        /// <param name="enemy">Enemy that receives the damage</param>
        void CauseDamage(int damage, IEnemy enemy);
    }
}