using Ecs.Data;
using Ecs.Systems;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

namespace Ecs
{
    public sealed class EcsGameStartup : MonoBehaviour
    {
        public StaticData configuration;
        public SceneData sceneData;
        private EcsSystems _systems;
        private EcsWorld _world;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _systems.ConvertScene();

            AddOneFrames();
            AddSystems();
            AddInjections();

            _systems.Init();
        }

        private void Update()
        {
            _systems.Run();
        }

        private void OnDestroy()
        {
            if (_systems == null) return;

            _systems.Destroy();
            _systems = null;

            _world.Destroy();
            _world = null;
        }

        private void AddInjections()
        {
            var runtimeData = new RuntimeData();

            _systems
                .Inject(configuration)
                .Inject(sceneData)
                .Inject(runtimeData)
                ;
        }

        private static void AddOneFrames()
        {
        }

        private void AddSystems()
        {
            _systems
                .Add(new PlayerInitSystem())
                .Add(new EnemyInitSystem())
                .Add(new SceneInitSystem())
                .Add(new PlayerJumpInitSystem())
                .Add(new InventoryInitSystem())
                .Add(new ChestInitSystem())
                .Add(new TemporaryInventoryInitSystem())
                .Add(new ShootingButtonInitSystem())
                .Add(new HealthBarSystem())
                .Add(new SpeedBarSystem())
                .Add(new PlayerInputSystem())
                .Add(new MovementSystem())
                .Add(new PlayerRightInputSystem())
                .Add(new PlayerLookSystem())
                .Add(new CheckInteractableSystem())
                .Add(new GenerateChestInventorySlotsSystem())
                .Add(new RenderPlayerInventorySystem())
                .Add(new UseInventoryItemsSystem())
                .Add(new PlayerShootingSystem())
                .Add(new BulletMovementSystem())
                .Add(new BulletDestroySystem())
                .Add(new EnemyShootingSystem())
                .Add(new EnemyMovingSystem())
                ;
        }
    }
}