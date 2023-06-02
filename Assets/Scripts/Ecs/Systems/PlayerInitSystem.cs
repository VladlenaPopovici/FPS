using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;
using Utils;

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

            playerEntity.Get<PlayerTag>();
            playerEntity.Get<PlayerComponent>();
            
            var healthBar = Object.Instantiate(staticData.healthBarImage, Constants.buttonsPanel);
            playerEntity.Get<HealthBarComponent>() = new HealthBarComponent()
            {
                hp = 50,
                hpBar = healthBar
            };

            var speedBar = Object.Instantiate(staticData.speedBarImage, Constants.buttonsPanel);
            playerEntity.Get<SpeedBarComponent>() = new SpeedBarComponent()
            {
                speedBarImage = speedBar
            };
            
            GameObject playerGO = Object.Instantiate(staticData.playerPrefab, sceneData.playerSpawnPoint.position, Quaternion.identity);
            ref var modelComponent = ref playerEntity.Get<ModelComponent>();
            modelComponent.modelTransform = playerGO.transform;
        }
    }
}