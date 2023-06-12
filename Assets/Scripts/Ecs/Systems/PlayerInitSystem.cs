using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ecs.Systems
{
    public sealed class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;
        private Button _jumpButton;

        private EcsFilter<PlayerComponent, JumpComponent> _jumpFilter;
        private SceneData _sceneData;
        private StaticData _staticData;


        public void Init()
        {
            var playerEntity = _world.NewEntity();

            playerEntity.Get<PlayerTag>();
            playerEntity.Get<PlayerComponent>() = new PlayerComponent();

            playerEntity.Get<JumpComponent>() = new JumpComponent
            {
                JumpForce = 5,
                Gravity = -9.8f
            };

            var healthBar = Object.Instantiate(_staticData.healthBarImage, Constants.ButtonsPanel);
            playerEntity.Get<HealthBarComponent>() = new HealthBarComponent
            {
                Hp = 50,
                HpBar = healthBar
            };

            var speedBar = Object.Instantiate(_staticData.speedBarImage, Constants.ButtonsPanel);
            playerEntity.Get<SpeedBarComponent>() = new SpeedBarComponent
            {
                SpeedBarImage = speedBar
            };

            var playerGo = Object.Instantiate(_staticData.playerPrefab, _sceneData.playerSpawnPoint.position,
                Quaternion.identity);
            ref var modelComponent = ref playerEntity.Get<ModelComponent>();
            modelComponent.modelTransform = playerGo.transform;

            playerEntity.Get<InteractableTag>();
            playerEntity.Get<InteractableComponent>() = new InteractableComponent
            {
                collider = playerGo.GetComponent<Collider>(),
                transform = playerGo.transform,
                type = InteractableType.Player
            };

            playerEntity.Get<WeaponComponent>() = new WeaponComponent
            {
                IsFullAuto = false,
                FireRate = 1f
            };
        }
    }
}