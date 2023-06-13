using Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ecs.Data
{
    [CreateAssetMenu]
    public class StaticData : ScriptableObject
    {
        [Header("prefabs")] public GameObject playerPrefab;
        public Button inventoryButtonPrefab;
        public ScrollRect inventoryScrollViewPrefab;
        public GameObject chestPrefab;
        public Button openChestButtonPrefab;
        public ScrollRect chestInventoryPrefab;
        public Button jumpButtonPrefab;
        public Button shootButtonPrefab;
        public GameObject bulletParentPrefab;
        public GameObject bulletPrefab;
        public GameObject enemyPrefab;
        public GameObject enemyChasingPrefab;

        [Header("configs")] public int inventoryCapacity;

        [Header("ray cast props")] public LayerMask layerMask;

        [Header("sprites")] public Sprite hpPotionImage;
        public Sprite speedPotionImage;
        public Sprite weaponImage;
        public Sprite emptyImage;
        public Image healthBarImage;
        public Image speedBarImage;

        [Header("environment")] public GameObject[] trees;
        public GameObject[] plants;
        public GameObject[] rocks;

        [Header("helper wrapper objects")] public GameObject parentNature;
        public GameObject parentBullet;
        public GameObject parentChest;

        public Sprite GetSpriteByItemType(ItemType? type)
        {
            return type switch
            {
                ItemType.HealthPotion => hpPotionImage,
                ItemType.SpeedPotion => speedPotionImage,
                ItemType.Weapon => weaponImage,
                _ => emptyImage
            };
        }
    }
}