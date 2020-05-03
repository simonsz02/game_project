namespace TowerDefenseGame.Model
{
    public interface IProjectile
    {
        void CauseDamage(int damage, IEnemy enemy);
    }
}