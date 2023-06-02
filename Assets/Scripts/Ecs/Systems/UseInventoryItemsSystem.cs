using Ecs.Data;
using Inventory;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class UseInventoryItemsSystem : IEcsRunSystem
    {
        private EcsFilter<InventoryTag, LatestClickedSlotComponent> _slotClickedFilter;
        private EcsFilter<PlayerTag, InventoryComponent> _playerInventoryFilter;
        private EcsFilter<PlayerTag, HealthBarComponent> _healthBarFilter;
        private EcsFilter<PlayerTag, SpeedBarComponent> _speedBarFilter;
        private StaticData _staticData;

        public void Run()
        {
            foreach (var i in _slotClickedFilter)
            {
                ref var latestClickedSlotComponent = ref _slotClickedFilter.Get2(i);

                foreach (var j in _playerInventoryFilter)
                {
                    ref var inventoryComponent = ref _playerInventoryFilter.Get2(j);
                    for (int k = 0; k < inventoryComponent.slotComponents.Count; k++)
                    {
                        var slot = inventoryComponent.slotComponents[k];
                        if (!latestClickedSlotComponent.slotMetaData.isHandled && latestClickedSlotComponent.slotMetaData.index == k)
                        {
                            latestClickedSlotComponent.slotMetaData.isHandled = true;
                            var itemType = slot.itemComponent?.item.itemType;
                            switch (itemType)
                            {
                                case ItemType.HealthPotion:
                                    ProcessHealthPotion(ref slot);
                                    break;
                                case ItemType.SpeedPotion:
                                    ProcessSpeedPotion(ref slot);
                                    break;
                                case ItemType.Weapon:
                                    // TODO
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void ProcessSpeedPotion(ref SlotComponent slot)
        {
            foreach (var i in _speedBarFilter)
            {
                ref var speedBarComponent = ref _speedBarFilter.Get2(i);
                speedBarComponent.fullBarValue = 1f;
                speedBarComponent.decreaseAmount = 0.2f;
                slot.itemComponent!.quantity--;
                if (slot.itemComponent.quantity == 0)
                {
                    slot.itemComponent = null;
                }
            }
        }

        private void ProcessHealthPotion(ref SlotComponent slotComponent)
        {
            foreach (var i in _healthBarFilter)
            {
                ref var healthBarComponent = ref _healthBarFilter.Get2(i);
                healthBarComponent.hp = Mathf.Min(healthBarComponent.hp + 10, 100);
                slotComponent.itemComponent!.quantity--;
                if (slotComponent.itemComponent.quantity == 0)
                {
                    slotComponent.itemComponent = null;
                }
            }
        }
    }
}