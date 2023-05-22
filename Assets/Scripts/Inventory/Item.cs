using UnityEngine;

namespace Inventory
{
    public class Item : ScriptableObject
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