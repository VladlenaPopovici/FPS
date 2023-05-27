using UnityEngine;

namespace Inventory
{
    public class Item
    {
        public ItemType itemType;
        public Sprite itemSprite;
    }

    public enum ItemType
    {
        HealthPotion,
        SpeedPotion,
        Weapon
    }
}