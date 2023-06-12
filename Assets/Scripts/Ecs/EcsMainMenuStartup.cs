using Ecs.Data;
using Ecs.Systems;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

namespace Ecs
{
    public sealed class EcsMainMenuStartup : MonoBehaviour
    {
        public MainMenuStaticData staticData;
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
                .Inject(runtimeData)
                .Inject(staticData)
                ;
        }

        private static void AddOneFrames()
        {
        }

        private void AddSystems()
        {
            _systems
                .Add(new MainMenuSystem())
                ;
        }
    }
}