using Leopotam.Ecs;
using UnityEngine.UI;

namespace Ecs.Systems
{
    public sealed class GenerateChestInventorySlotsSystem : IEcsRunSystem
    {
        private EcsFilter<InteractableTag, ChestTag, InventoryComponent, ScrollViewComponent> _chestInventoryFilter;

        public void Run()
        {
            foreach (var i in _chestInventoryFilter)
            {
                ref var inventoryComponent = ref _chestInventoryFilter.Get3(i);
                for (var j = 0; j < inventoryComponent.slotComponents.Count; j++)
                {
                    var slot = inventoryComponent.slotComponents[j];
                    
                    if (!ChestHasItem(slot) || !IsChestOpened(_chestInventoryFilter.Get4(i))) continue;
                    
                    // ReSharper disable once PossibleInvalidOperationException
                    var itemComponent = slot.itemComponent.Value;
                    ref var scrollView = ref _chestInventoryFilter.Get4(i).scrollView;
                    var itemSprite = itemComponent.item.itemSprite;
                    var targetImage = scrollView.content.GetChild(j).GetChild(0).GetComponent<Image>();
                    targetImage.sprite = itemSprite;
                }
            }
        }

        private static bool IsChestOpened(ScrollViewComponent chestScrollView)
        {
            return chestScrollView.scrollView.IsActive();
        }

        private static bool ChestHasItem(SlotComponent slot)
        {
            return slot.itemComponent.HasValue;
        }
    }
}