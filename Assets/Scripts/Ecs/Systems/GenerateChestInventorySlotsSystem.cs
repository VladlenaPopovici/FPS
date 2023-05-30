using Ecs.Data;
using Inventory;
using Leopotam.Ecs;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Systems
{
    public sealed class GenerateChestInventorySlotsSystem : IEcsRunSystem
    {
        private EcsFilter<InteractableTag, ChestTag, InventoryComponent, ScrollViewComponent, InteractableComponent>
            _chestInventoryFilter;

        private EcsFilter<ChestTag, LatestClickedSlotComponent> _latestClickedChestSlotFilter;
        private EcsFilter<TemporaryInventoryComponent> _temporaryInventoryFilter;
        private StaticData _staticData;

        public void Run()
        {
            SlotMetaData slotMetaData = null;

            foreach (var i in _latestClickedChestSlotFilter)
            {
                ref var latestClickedSlotComponent = ref _latestClickedChestSlotFilter.Get2(i);
                slotMetaData = latestClickedSlotComponent.slotMetaData;
            }

            foreach (var i in _chestInventoryFilter)
            {
                ref var interactable = ref _chestInventoryFilter.Get5(i);

                if (!interactable.isNear) continue;
                if (!IsChestOpened(_chestInventoryFilter.Get4(i))) continue;

                ref var inventoryComponent = ref _chestInventoryFilter.Get3(i);
                ref var scrollView = ref _chestInventoryFilter.Get4(i).scrollView;

                for (var j = 0; j < inventoryComponent.slotComponents.Count; j++)
                {
                    var slot = inventoryComponent.slotComponents[j];

                    var itemComponent = slot.itemComponent;
                    
                    // empty slot
                    if (itemComponent == null)
                    {
                        var image = scrollView.content.GetChild(j).GetChild(0).GetComponent<Image>();
                        image.overrideSprite = _staticData.emptySprite;
                        continue;
                    }

                    if (slotMetaData != null && !slotMetaData.isHandled && slotMetaData.index == j)
                    {
                        foreach (var k in _temporaryInventoryFilter)
                        {
                            ref var temporaryInventoryComponent = ref _temporaryInventoryFilter.Get1(k);
                            temporaryInventoryComponent.transferedItem = itemComponent.item.itemType;
                        }
                        slotMetaData.isHandled = true;
                        if (itemComponent.quantity == 1)
                        {
                            slot.itemComponent = null;
                            slot.itemSprite = _staticData.emptySprite;
                            scrollView.content.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";                            
                            continue;
                        }

                        itemComponent.quantity--;
                    }

                    var itemSprite = slot.itemSprite;
                    var targetImage = scrollView.content.GetChild(j).GetChild(0).GetComponent<Image>();
                    targetImage.overrideSprite = itemSprite;

                    if (itemComponent.item.itemType == ItemType.Weapon) continue;

                    var textQuantity = scrollView.content.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>();
                    textQuantity.text = itemComponent.quantity.ToString();
                }
            }
        }

        private static bool IsChestOpened(ScrollViewComponent chestScrollView)
        {
            return chestScrollView.scrollView.IsActive();
        }
    }
}