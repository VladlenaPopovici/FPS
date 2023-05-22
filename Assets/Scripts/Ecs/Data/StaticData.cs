using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
    }
}