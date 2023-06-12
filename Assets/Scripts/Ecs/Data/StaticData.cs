using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Data
{
    [CreateAssetMenu]
    public class StaticData : ScriptableObject
    {
        public GameObject playerPrefab;
        public int inventoryCapacity;
        public Button inventoryButtonPrefab;
        public ScrollRect inventoryScrollViewPrefab;
        public GameObject chestPrefab;
        public GameObject parentChest;
        public LayerMask layerMask;
        public Button openChestButtonPrefab;
        public ScrollRect chestInventoryPrefab;
        public Sprite hpPotionImage;
        public Sprite speedPotion;
        public Sprite weaponImage;
        public Sprite emptySprite;
        public Image healthBarImage;
        public Image speedBarImage;

        public Button jumpButtonPrefab;
        public Button shootButtonPrefab;
        public GameObject bulletParentPrefab;
        public GameObject bulletPrefab;

        public GameObject parentNature;
        public GameObject[] trees;
        public GameObject[] plants;
        public GameObject[] rocks;

        public GameObject enemyPrefab;
        public GameObject enemyChasingPrefab;

        public GameObject bulletParent;

        public Sprite GetSpriteByItemType(ItemType? type)
        {
            return type switch
            {
                ItemType.HealthPotion => hpPotionImage,
                ItemType.SpeedPotion => speedPotion,
                ItemType.Weapon => weaponImage,
                _ => emptySprite
            };
        }
    }
}