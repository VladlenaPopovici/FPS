using Leopotam.Ecs;

namespace Ecs.Systems
{
    public class TemporaryInventoryInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        
        public void Init()
        {
            var temporaryInventoryEntity = _world.NewEntity();
            temporaryInventoryEntity.Get<TemporaryInventoryComponent>();
        }
    }
}