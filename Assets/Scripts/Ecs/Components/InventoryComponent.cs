using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Ecs
{
    public struct InventoryComponent
    {
        public List<SlotComponent> slotComponents;

        public bool IsFitting(ItemType itemType)
        {
            foreach (var slotComponent in slotComponents)
            {
                if (slotComponent.itemComponent == null)
                {
                    return true;
                }

                if (slotComponent.itemComponent.item.itemType == itemType && itemType != ItemType.Weapon)
                {
                    return true;
                }
            }

            return false;
        }
    }
    
    public class SlotComponent
    {
        public ItemComponent? itemComponent;
        public Sprite itemSprite;
    }
}