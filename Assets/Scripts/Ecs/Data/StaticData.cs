using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Data
{
    [CreateAssetMenu]
    public class StaticData : ScriptableObject
    {
        public List<GameObject> weaponPrefabs;
        public GameObject playerPrefab;
        public Vector3 weaponOffset;
    }
}