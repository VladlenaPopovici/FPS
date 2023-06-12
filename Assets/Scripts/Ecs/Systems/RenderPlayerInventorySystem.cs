using System;
using System.Collections.Generic;
using System.Linq;
using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Inventory;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Systems
{
    public class RenderPlayerInventorySystem : IEcsRunSystem
    {
        private EcsFilter<PlayerTag, InventoryTag, InventoryComponent, ScrollViewComponent> _playerInventoryFilter;

        private StaticData _staticData;
        private EcsFilter<TemporaryInventoryComponent> _temporaryInventoryFilter;

        public void Run()
        {
            foreach (var i in _playerInventoryFilter)
            {
                var inventoryComponent = _playerInventoryFilter.Get3(i);
                ref var scrollViewComponent = ref _playerInventoryFilter.Get4(i);

                TemporaryInventoryComponent? temporaryInventory = null;
                foreach (var j in _temporaryInventoryFilter) temporaryInventory = _temporaryInventoryFilter.Get1(j);

                if (temporaryInventory is { TransferredItem: { } })
                {
                    switch (temporaryInventory.Value.TransferredItem)
                    {
                        case ItemType.HealthPotion:
                        case ItemType.SpeedPotion:
                        {
                            // Find slot with potion if none find empty
                            // Increment quantity by one or insert new item with quantity one
                            var slotToAssign = FindSuitableSlot(
                                inventoryComponent.SlotComponents,
                                temporaryInventory.Value.TransferredItem.Value
                            );
                            if (slotToAssign == null)
                            {
                                Debug.Log("Could not find empty slot");
                            }
                            else if (slotToAssign.ItemComponent == null)
                            {
                                Debug.Log("found empty slot");
                                slotToAssign.ItemSprite =
                                    _staticData.GetSpriteByItemType(temporaryInventory.Value.TransferredItem);
                                slotToAssign.ItemComponent = new ItemComponent
                                {
                                    Quantity = 1,
                                    Item = new Item
                                    {
                                        ItemType = temporaryInventory.Value.TransferredItem.Value
                                    }
                                };
                            }
                            else
                            {
                                slotToAssign.ItemComponent.Quantity++;
                            }

                            break;
                        }
                        case ItemType.Weapon:
                        {
                            //Find empty slot and insert new item
                            var slotToAssign = FindEmptySlot(inventoryComponent.SlotComponents);
                            if (slotToAssign == null)
                            {
                                Debug.Log("Could not find empty slot");
                            }
                            else
                            {
                                slotToAssign.ItemSprite =
                                    _staticData.GetSpriteByItemType(temporaryInventory.Value.TransferredItem);
                                slotToAssign.ItemComponent = new ItemComponent
                                {
                                    Quantity = 1,
                                    Item = new Item
                                    {
                                        ItemType = temporaryInventory.Value.TransferredItem.Value
                                    }
                                };
                            }

                            break;
                        }
                        case null:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    foreach (var j in _temporaryInventoryFilter)
                    {
                        ref var temporaryInventoryComponent = ref _temporaryInventoryFilter.Get1(j);
                        temporaryInventoryComponent.TransferredItem = null;
                    }
                }


                for (var j = 0; j < inventoryComponent.SlotComponents.Count; j++)
                {
                    var slot = inventoryComponent.SlotComponents[j];

                    // empty slot
                    if (slot.ItemComponent == null)
                    {
                        var image = scrollViewComponent.ScrollView.content.GetChild(j).GetChild(0)
                            .GetComponent<Image>();
                        image.overrideSprite = _staticData.emptySprite;
                        scrollViewComponent.ScrollView.content.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>()
                            .text = "";
                        continue;
                    }

                    scrollViewComponent.ScrollView.content.GetChild(j).GetChild(0).GetComponent<Image>()
                        .overrideSprite = slot.ItemSprite;

                    if (slot.ItemComponent == null || slot.ItemComponent.Item.ItemType == ItemType.Weapon) continue;

                    scrollViewComponent.ScrollView.content.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = slot.ItemComponent.Quantity.ToString();
                }
            }
        }

        private SlotComponent FindEmptySlot(List<SlotComponent> slots)
        {
            return slots.FirstOrDefault(slot => slot.ItemComponent == null);
        }

        private SlotComponent FindSuitableSlot(List<SlotComponent> slots, ItemType itemType)
        {
            var slotComponent = slots.FirstOrDefault(slot => slot.ItemComponent?.Item.ItemType == itemType);
            if (slotComponent != null) return slotComponent;

            return slots.FirstOrDefault(slot => slot.ItemComponent == null);
        }
    }
}