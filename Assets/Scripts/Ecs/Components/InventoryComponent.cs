using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Ecs
{
    public struct InventoryComponent
    {
        public List<SlotComponent> slotComponents;
        public ButtonComponent inventoryButton;
        public ScrollRect inventoryScrollView;
    }
}