namespace TowerDefenseGame.Model.Interface
{
    interface IGameItem
    {
        int Health { get; set; }
        int Armour { get; set; }
        int Range { get; set; }
        IGameItem Target { get; set; }
    }
}
