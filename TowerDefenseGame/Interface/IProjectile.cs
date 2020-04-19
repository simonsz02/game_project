namespace TowerDefenseGame
{
    public interface IProjectile
    {
        void CauseDamage(int damage, IEnemy enemy);
    }
}