namespace Inventory
{
    public class Item
    {
        public ItemType ItemType;
    }

    public enum ItemType
    {
        HealthPotion,
        SpeedPotion,
        Weapon
    }
}