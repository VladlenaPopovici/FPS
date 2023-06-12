using System;
using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Inventory;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class UseInventoryItemsSystem : IEcsRunSystem
    {
        private EcsFilter<PlayerTag, HealthBarComponent> _healthBarFilter;
        private EcsFilter<PlayerTag, InventoryComponent> _playerInventoryFilter;
        private EcsFilter<InventoryTag, LatestClickedSlotComponent> _slotClickedFilter;
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
                    for (var k = 0; k < inventoryComponent.SlotComponents.Count; k++)
                    {
                        var slot = inventoryComponent.SlotComponents[k];
                        if (!latestClickedSlotComponent.SlotMetaData.IsHandled &&
                            latestClickedSlotComponent.SlotMetaData.Index == k)
                        {
                            latestClickedSlotComponent.SlotMetaData.IsHandled = true;
                            var itemType = slot.ItemComponent?.Item.ItemType;
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
                                case null:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
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
                speedBarComponent.FullBarValue = 1f;
                speedBarComponent.DecreaseAmount = 0.2f;
                slot.ItemComponent!.Quantity--;
                if (slot.ItemComponent.Quantity == 0) slot.ItemComponent = null;
            }
        }

        private void ProcessHealthPotion(ref SlotComponent slotComponent)
        {
            foreach (var i in _healthBarFilter)
            {
                ref var healthBarComponent = ref _healthBarFilter.Get2(i);
                healthBarComponent.Hp = Mathf.Min(healthBarComponent.Hp + 10, 100);
                slotComponent.ItemComponent!.Quantity--;
                if (slotComponent.ItemComponent.Quantity == 0) slotComponent.ItemComponent = null;
            }
        }
    }
}