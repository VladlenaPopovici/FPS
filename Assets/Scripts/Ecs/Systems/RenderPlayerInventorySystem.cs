using System.Collections.Generic;
using System.Linq;
using Ecs.Data;
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
        private EcsFilter<TemporaryInventoryComponent> _temporaryInventoryFilter;

        private StaticData _staticData;

        public void Run()
        {
            foreach (var i in _playerInventoryFilter)
            {
                var inventoryComponent = _playerInventoryFilter.Get3(i);
                ref var scrollViewComponent = ref _playerInventoryFilter.Get4(i);

                TemporaryInventoryComponent? temporaryInventory = null;
                foreach (var j in _temporaryInventoryFilter)
                {
                    temporaryInventory = _temporaryInventoryFilter.Get1(j);
                }

                if (temporaryInventory != null && temporaryInventory.Value.transferedItem != null)
                {
                    switch (temporaryInventory.Value.transferedItem)
                    {
                        case ItemType.HealthPotion:
                        case ItemType.SpeedPotion:
                        {
                            // Find slot with potion if none find empty
                            // Increment quantity by one or insert new item with quantity one
                            var slotToAssign = FindSuitableSlot(
                                inventoryComponent.slotComponents,
                                temporaryInventory.Value.transferedItem.Value
                            );
                            if (slotToAssign == null)
                            {
                                Debug.Log("Could not find empty slot");
                            }
                            else if (slotToAssign.itemComponent == null)
                            {
                                Debug.Log("found empty slot");
                                slotToAssign.itemSprite =
                                    _staticData.GetSpriteByItemType(temporaryInventory.Value.transferedItem);
                                slotToAssign.itemComponent = new ItemComponent()
                                {
                                    quantity = 1,
                                    item = new Item()
                                    {
                                        itemType = temporaryInventory.Value.transferedItem.Value
                                    }
                                };
                            }
                            else
                            {
                                Debug.Log("found non empty slot");
                                slotToAssign.itemComponent.quantity++;
                            }

                            break;
                        }
                        case ItemType.Weapon:
                        {
                            //Find empty slot and insert new item
                            var slotToAssign = FindEmptySlot(inventoryComponent.slotComponents);
                            if (slotToAssign == null)
                            {
                                Debug.Log("Could not find empty slot");
                            }
                            else
                            {
                                slotToAssign.itemSprite =
                                    _staticData.GetSpriteByItemType(temporaryInventory.Value.transferedItem);
                                slotToAssign.itemComponent = new ItemComponent()
                                {
                                    quantity = 1,
                                    item = new Item()
                                    {
                                        itemType = temporaryInventory.Value.transferedItem.Value
                                    }
                                };
                            }
                            break;
                        }
                    }

                    foreach (var j in _temporaryInventoryFilter)
                    {
                        ref var temporaryInventoryComponent = ref _temporaryInventoryFilter.Get1(j);
                        temporaryInventoryComponent.transferedItem = null;
                    }
                }


                for (int j = 0; j < inventoryComponent.slotComponents.Count; j++)
                {
                    var slot = inventoryComponent.slotComponents[j];
                    
                    // empty slot
                    if (slot.itemComponent == null)
                    {
                        var image = scrollViewComponent.scrollView.content.GetChild(j).GetChild(0).GetComponent<Image>();
                        image.overrideSprite = _staticData.emptySprite;
                        scrollViewComponent.scrollView.content.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; 
                        continue;
                    }
                    
                    scrollViewComponent.scrollView.content.GetChild(j).GetChild(0).GetComponent<Image>()
                        .overrideSprite = slot.itemSprite;

                    if (slot.itemComponent == null || slot.itemComponent.item.itemType == ItemType.Weapon) continue;

                    scrollViewComponent.scrollView.content.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>()
                        .text = slot.itemComponent.quantity.ToString();
                }
            }
        }

        private SlotComponent FindEmptySlot(List<SlotComponent> slots)
        {
            return slots.FirstOrDefault(slot => slot.itemComponent == null);
        }

        private SlotComponent FindSuitableSlot(List<SlotComponent> slots, ItemType itemType)
        {
            var slotComponent = slots.FirstOrDefault(slot => slot.itemComponent?.item.itemType == itemType);
            if (slotComponent != null)
            {
                return slotComponent;
            }

            return slots.FirstOrDefault(slot => slot.itemComponent == null);
        }
    }
}