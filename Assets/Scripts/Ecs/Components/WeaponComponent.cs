using JetBrains.Annotations;
using UnityEngine;

namespace Ecs.Components
{
    public struct WeaponComponent
    {
        public bool IsFullAuto;
        public float? FireRate;
        public float LastFireTimestamp;
        [CanBeNull] public Transform WeaponTransform;
    }
}