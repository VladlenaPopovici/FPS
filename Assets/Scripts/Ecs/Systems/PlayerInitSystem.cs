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
            // ref var inputData = ref playerEntity.Get<PlayerInputData>();
            
            GameObject playerGO = Object.Instantiate(staticData.playerPrefab, sceneData.playerSpawnPoint.position, Quaternion.identity);
            player.playerCharacterController = playerGO.GetComponent<CharacterController>();
        }
    }
}