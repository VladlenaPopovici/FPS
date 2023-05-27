using System.Collections.Generic;
using Ecs.Data;
using Inventory;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ecs
{
    public sealed class ChestInitSystem : IEcsInitSystem
    {
        private const float MinX = -50;
        private const float MaxX = 50;
        private const float MinZ = -50;
        private const float MaxZ = 50;

        private EcsFilter<InventoryTag, ButtonComponent, ScrollViewComponent> _inventoryFilter;

        private EcsWorld _world;
        private StaticData _staticData;
        private ScrollRect _chestInventory;
        private Button _openChestButton;

        public void Init()
        {
            var parentChest = Object.Instantiate(_staticData.parentChest);
            _chestInventory = Object.Instantiate(_staticData.chestInventoryPrefab, Constants.buttonsPanel);

            for (var i = 0; i < 10; i++)
            {
                var position = new Vector3
                {
                    x = Randomizer.GetRandomInRange(MinX, MaxX),
                    z = Randomizer.GetRandomInRange(MinZ, MaxZ),
                    y = 0.3f
                };

                var rotationVector = new Vector3
                {
                    y = Randomizer.GetRandomInRange(Constants.MinAngle, Constants.MaxAngle)
                };
                var rotation = Quaternion.Euler(rotationVector);


                var chest = Object.Instantiate(_staticData.chestPrefab, position, rotation, parentChest.transform);

                var chestEntity = _world.NewEntity();
                chestEntity.Get<ChestTag>();
                chestEntity.Get<InteractableTag>();
                chestEntity.Get<InventoryTag>();
                chestEntity.Get<InteractableComponent>() = new InteractableComponent()
                {
                    collider = chest.GetComponent<Collider>(),
                    transform = chest.transform,
                    type = InteractableType.Chest
                };

                chestEntity.Get<ScrollViewComponent>() = new ScrollViewComponent()
                {
                    scrollView = _chestInventory
                };
                
                var inventoryComponent = new InventoryComponent();
                inventoryComponent.slotComponents = new List<SlotComponent>(_staticData.inventoryCapacity);
                for (var j = 0; j < _staticData.inventoryCapacity; j++)
                {
                    inventoryComponent.slotComponents.Add(GenerateRandomSlot());
                }

                chestEntity.Get<InventoryComponent>() = inventoryComponent;
            }
            
            var chestButtonEntity = _world.NewEntity();

            _openChestButton = Object.Instantiate(_staticData.openChestButtonPrefab, Constants.buttonsPanel);
            _openChestButton.onClick.AddListener(OpenChestInventory);
            chestButtonEntity.Get<ButtonComponent>() = new ButtonComponent()
            {
                button = _openChestButton
            };
            chestButtonEntity.Get<OpenChestButtonTag>();

            _chestInventory.GetComponentInChildren<Button>().onClick.AddListener(CloseChestInventory);
        }

        private SlotComponent GenerateRandomSlot()
        {
            var slotComponent = new SlotComponent();

            if (Randomizer.GetRandomBool())
            {
                var itemComponent = new ItemComponent();
                itemComponent.item = GenerateRandomItem();
                itemComponent.quantity = (byte)Randomizer.GetRandomInRange(1, 5);
                if (itemComponent.item.itemType == ItemType.Weapon)
                {
                    itemComponent.quantity = 1;
                }
                
                slotComponent.itemComponent = itemComponent;
            }

            return slotComponent;
        }

        private Item GenerateRandomItem()
        {
            var item = new Item
            {
                itemType = Randomizer.GetRandomEnumValue<ItemType>(),
                // item.itemSprite = _staticData.ItemTypeSprite[item.itemType];
                itemSprite = _staticData.hpPotionImage
            };
            return item;
        }

        private void OpenChestInventory()
        {
            Time.timeScale = 0;
            _chestInventory.gameObject.SetActive(true);
            _openChestButton.gameObject.SetActive(false);
            foreach (var i in _inventoryFilter)
            {
                var inventoryButton = _inventoryFilter.Get2(i);
                inventoryButton.button.gameObject.SetActive(false);
                var inventoryScrollView = _inventoryFilter.Get3(i);
                inventoryScrollView.scrollView.gameObject.SetActive(true);
            }
        }

        private void CloseChestInventory()
        {
            Time.timeScale = 1;
            _chestInventory.gameObject.SetActive(false);
            _openChestButton.gameObject.SetActive(true);
            foreach (var i in _inventoryFilter)
            {
                var inventoryButton = _inventoryFilter.Get2(i);
                inventoryButton.button.gameObject.SetActive(true);
                var inventoryScrollView = _inventoryFilter.Get3(i);
                inventoryScrollView.scrollView.gameObject.SetActive(false);
            }
        }
    }
}