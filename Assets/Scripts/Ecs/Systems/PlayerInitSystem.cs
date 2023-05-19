using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs
{
    public sealed class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;
        private StaticData staticData; 
        private SceneData sceneData;
        
        public void Init()
        {
            EcsEntity playerEntity = _world.NewEntity();

            ref var player = ref playerEntity.Get<PlayerComponent>();

            GameObject playerGO = Object.Instantiate(staticData.playerPrefab, sceneData.playerSpawnPoint.position, Quaternion.identity);
            ref var modelComponent = ref playerEntity.Get<ModelComponent>();
            modelComponent.modelTransform = playerGO.transform;
            
            GameObject weaponModel = Object.Instantiate(staticData.weaponPrefabs[0]);
            ref var weaponModelComponent = ref playerEntity.Get<WeaponModelComponent>();
            weaponModelComponent.transform = weaponModel.transform;            
            
            // Get the WeaponOffset Component from the player entity
            playerEntity.Get<WeaponOffsetComponent>() = new WeaponOffsetComponent()
            {
                rotationOffset = Quaternion.Euler(-7.47f, -7, 0),
                positionOffset = playerGO.transform.position + staticData.weaponOffset,
            };
        }
    }
}