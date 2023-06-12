using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Inventory;
using Leopotam.Ecs;
using TMPro;
using UnityEngine.UI;

namespace Ecs.Systems
{
    public sealed class GenerateChestInventorySlotsSystem : IEcsRunSystem
    {
        private EcsFilter<InteractableTag, ChestTag, InventoryComponent, ScrollViewComponent, InteractableComponent>
            _chestInventoryFilter;

        private EcsFilter<ChestTag, LatestClickedSlotComponent> _latestClickedChestSlotFilter;
        private EcsFilter<PlayerTag, InventoryTag, InventoryComponent> _playerInventoryFilter;

        private StaticData _staticData;
        private EcsFilter<TemporaryInventoryComponent> _temporaryInventoryFilter;

        public void Run()
        {
            SlotMetaData slotMetaData = null;

            foreach (var i in _latestClickedChestSlotFilter)
            {
                ref var latestClickedSlotComponent = ref _latestClickedChestSlotFilter.Get2(i);
                slotMetaData = latestClickedSlotComponent.SlotMetaData;
            }

            foreach (var i in _chestInventoryFilter)
            {
                ref var interactable = ref _chestInventoryFilter.Get5(i);

                if (!interactable.isNear) continue;
                if (!IsChestOpened(_chestInventoryFilter.Get4(i))) continue;

                ref var inventoryComponent = ref _chestInventoryFilter.Get3(i);
                ref var scrollView = ref _chestInventoryFilter.Get4(i).ScrollView;

                for (var j = 0; j < inventoryComponent.SlotComponents.Count; j++)
                {
                    var slot = inventoryComponent.SlotComponents[j];

                    var itemComponent = slot.ItemComponent;

                    // empty slot
                    if (itemComponent == null)
                    {
                        var image = scrollView.content.GetChild(j).GetChild(0).GetComponent<Image>();
                        image.overrideSprite = _staticData.emptySprite;
                        scrollView.content.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                        continue;
                    }

                    if (slotMetaData != null && !slotMetaData.IsHandled && slotMetaData.Index == j)
                    {
                        var isFitting = true;
                        foreach (var k in _playerInventoryFilter)
                        {
                            var playerInventoryComponent = _playerInventoryFilter.Get3(k);
                            isFitting = playerInventoryComponent.IsFitting(itemComponent.Item.ItemType);
                        }

                        if (!isFitting) continue;

                        foreach (var k in _temporaryInventoryFilter)
                        {
                            ref var temporaryInventoryComponent = ref _temporaryInventoryFilter.Get1(k);
                            temporaryInventoryComponent.TransferredItem = itemComponent.Item.ItemType;
                        }

                        slotMetaData.IsHandled = true;
                        if (itemComponent.Quantity == 1)
                        {
                            slot.ItemComponent = null;
                            slot.ItemSprite = _staticData.emptySprite;
                            scrollView.content.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                            continue;
                        }

                        itemComponent.Quantity--;
                    }

                    var itemSprite = slot.ItemSprite;
                    var targetImage = scrollView.content.GetChild(j).GetChild(0).GetComponent<Image>();
                    targetImage.overrideSprite = itemSprite;

                    if (itemComponent.Item.ItemType == ItemType.Weapon) continue;

                    var textQuantity = scrollView.content.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>();
                    textQuantity.text = itemComponent.Quantity.ToString();
                }
            }
        }

        private static bool IsChestOpened(ScrollViewComponent chestScrollView)
        {
            return chestScrollView.ScrollView.IsActive();
        }
    }
}