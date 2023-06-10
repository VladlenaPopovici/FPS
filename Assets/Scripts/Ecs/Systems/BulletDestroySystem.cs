using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class BulletDestroySystem : IEcsRunSystem
    {
        private EcsFilter<BulletComponent> _bulletFilter;

        public void Run()
        {
            foreach (var i in _bulletFilter)
            {
                var bulletComponent = _bulletFilter.Get1(i);

                if (!bulletComponent.isDespawned) continue;
                
                DestroyAnimation();
                Object.Destroy(bulletComponent.gameObject);
                var bulletEntity = _bulletFilter.GetEntity(i);
                bulletEntity.Destroy();
            }
        }

        private void DestroyAnimation()
        {
            // TODO add animation
        }
    }
}