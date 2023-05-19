using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs
{
    public sealed class PlayerWeaponChaseSystem : IEcsRunSystem
    {
        private StaticData _staticData;
        
        private readonly EcsFilter<WeaponModelComponent> weaponFilter = null;
        private readonly EcsFilter<PlayerComponent> playerFilter = null; 

        public void Run()
        {
            Vector3 playerPosition = new Vector3();

            // TODO check if this may be optimized with saving the ref from init
            foreach (var i in playerFilter)
            {
                ref var playerEntity = ref playerFilter.GetEntity(0);
                ref var model = ref playerEntity.Get<ModelComponent>();
                playerPosition = model.modelTransform.position;
            }
            
            foreach (var i in weaponFilter)
            {
                ref var weaponModelComponent = ref weaponFilter.Get1(i);
                weaponModelComponent.transform.position = playerPosition + _staticData.weaponOffset;
            }
        }
    }
}