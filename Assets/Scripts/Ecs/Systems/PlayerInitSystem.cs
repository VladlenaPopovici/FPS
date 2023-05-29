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

            playerEntity.Get<PlayerComponent>();

            GameObject playerGO = Object.Instantiate(staticData.playerPrefab, sceneData.playerSpawnPoint.position, Quaternion.identity);
            ref var modelComponent = ref playerEntity.Get<ModelComponent>();
            modelComponent.modelTransform = playerGO.transform;
        }
    }
}