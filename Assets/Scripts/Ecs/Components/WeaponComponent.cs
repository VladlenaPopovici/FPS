using JetBrains.Annotations;
using UnityEngine;

namespace Ecs
{
    public struct WeaponComponent
    {
        public bool isFullAuto;
        public float? fireRate;
        public float lastFireTimestamp;
        [CanBeNull] public Transform weaponTransform;
    }
}