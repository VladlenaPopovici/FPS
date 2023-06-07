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
        private StaticData _staticData; 
        private SceneData _sceneData;
        private Button _jumpButton;
        
        private EcsFilter<PlayerComponent, JumpComponent> _jumpFilter;


        public void Init()
        {
            var playerEntity = _world.NewEntity();

            playerEntity.Get<PlayerTag>();
            playerEntity.Get<PlayerComponent>() = new PlayerComponent()
            {
                playerCharacterController = _staticData.characterController
            };

            playerEntity.Get<JumpComponent>() = new JumpComponent()
            {
                jumpForce = 500,
                gravity = -9.8f,
            };
            
            var healthBar = Object.Instantiate(_staticData.healthBarImage, Constants.buttonsPanel);
            playerEntity.Get<HealthBarComponent>() = new HealthBarComponent()
            {
                hp = 50,
                hpBar = healthBar
            };

            var speedBar = Object.Instantiate(_staticData.speedBarImage, Constants.buttonsPanel);
            playerEntity.Get<SpeedBarComponent>() = new SpeedBarComponent()
            {
                speedBarImage = speedBar
            };
            
            var playerGo = Object.Instantiate(_staticData.playerPrefab, _sceneData.playerSpawnPoint.position, Quaternion.identity);
            ref var modelComponent = ref playerEntity.Get<ModelComponent>();
            modelComponent.modelTransform = playerGo.transform;

            playerEntity.Get<WeaponComponent>() = new WeaponComponent()
            {
                isFullAuto = true,
                fireRate = 1f
            };
        }
    }
}