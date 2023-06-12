using System.Collections.Generic;
using Inventory;
using JetBrains.Annotations;
using UnityEngine;

namespace Ecs.Components
{
    public struct InventoryComponent
    {
        public List<SlotComponent> SlotComponents;

        public bool IsFitting(ItemType itemType)
        {
            foreach (var slotComponent in SlotComponents)
            {
                if (slotComponent.ItemComponent == null) return true;

                if (slotComponent.ItemComponent.Item.ItemType == itemType && itemType != ItemType.Weapon) return true;
            }

            return false;
        }
    }

    public class SlotComponent
    {
        [CanBeNull] public ItemComponent ItemComponent;
        public Sprite ItemSprite;
    }
}