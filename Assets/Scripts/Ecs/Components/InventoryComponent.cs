using System.Collections.Generic;
using UnityEngine;

namespace Ecs
{
    public struct InventoryComponent
    {
        public List<SlotComponent> slotComponents;
    }
    
    public class SlotComponent
    {
        public ItemComponent? itemComponent;
        public Sprite itemSprite;
    }
}