using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utils;
using Button = UnityEngine.UI.Button;

namespace Ecs.Data
{
    [CreateAssetMenu]
    public class StaticData : ScriptableObject
    {
        public List<GameObject> weaponPrefabs;
        public GameObject playerPrefab;
        public Vector3 weaponOffset;
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
        // public SerializableDictionary<ItemType, Sprite> ItemTypeSprite;
    }
}