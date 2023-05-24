using System.Collections.Generic;
using Ecs.Data;
using Leopotam.Ecs;

namespace Ecs
{
    public sealed class InventoryInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;
        private StaticData _staticData;


        public void Init()
        {
            EcsEntity inventoryEntity = _world.NewEntity();
            inventoryEntity.Get<InventoryTag>();
            InventoryComponent inventoryComponent = inventoryEntity.Get<InventoryComponent>();
            inventoryComponent.slotComponents = new List<SlotComponent>(_staticData.inventoryCapacity);

            for (int i = 0; i < _staticData.inventoryCapacity; i++)
            {
                inventoryComponent.slotComponents.Add(new SlotComponent());
            }
        }
    }
}