using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class BulletMovementSystem : IEcsRunSystem
    {
        private EcsFilter<BulletComponent> _bulletFilter;
        
        public void Run()
        {
            foreach (var i in _bulletFilter)
            {
                ref var bulletComponent = ref _bulletFilter.Get1(i);
            }
        }
    }
}