using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ecs
{
    public sealed class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;
        private StaticData staticData; 
        private SceneData sceneData;
        private Button _jumpButton;
        
        private EcsFilter<PlayerComponent, JumpComponent> _jumpFilter;


        public void Init()
        {
            EcsEntity playerEntity = _world.NewEntity();

            playerEntity.Get<PlayerTag>();
            playerEntity.Get<PlayerComponent>() = new PlayerComponent()
            {
                playerCharacterController = staticData.characterController
            };

            playerEntity.Get<JumpComponent>() = new JumpComponent()
            {
                jumpForce = 500,
                gravity = -9.8f,
            };
            
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

        private void Jump()
        {
            
        }
    }
}