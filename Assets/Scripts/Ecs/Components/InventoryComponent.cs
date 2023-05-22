﻿using System.Collections.Generic;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Ecs
{
    public struct InventoryComponent
    {
        public List<SlotComponent> slotComponents;
        public ButtonComponent inventoryButton;
        public ScrollRect inventoryScrollView;
        public Button closeInventoryButton;
    }
}