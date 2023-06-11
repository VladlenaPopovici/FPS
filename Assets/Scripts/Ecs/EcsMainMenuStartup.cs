using Ecs.Data;
using Ecs.Systems;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

namespace Ecs
{
    public sealed class EcsMainMenuStartup : MonoBehaviour
    {
        private EcsWorld world;
        private EcsSystems systems;

        public MainMenuStaticData _staticData;
        
        // Start is called before the first frame update
        void Start()
        {
            world = new EcsWorld();
            systems = new EcsSystems(world);
        
            systems.ConvertScene();
        
            AddOneFrames();
            AddSystems();
            AddInjections();

            systems.Init();
        }

        private void AddInjections()
        {
            var runtimeData = new RuntimeData();

            systems
                .Inject(runtimeData)
                .Inject(_staticData)
                ;
        }

        private void AddOneFrames()
        {
        
        }

        private void AddSystems()
        {
            systems
                .Add(new MainMenuSystem())
                ;
        }

        // Update is called once per frame
        void Update()
        {
            systems.Run();
        }

        private void OnDestroy()
        {
            if (systems == null)
            {
                return;
            }
        
            systems.Destroy();
            systems = null;
        
            world.Destroy();
            world = null;
        }
    }
}