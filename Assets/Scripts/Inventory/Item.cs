using UnityEngine;

namespace Inventory
{
    public class Item
    {
        public ItemType itemType;
    }

    public enum ItemType
    {
        HealthPotion,
        SpeedPotion,
        Weapon
    }
}