using System;
using System.Collections.Generic;
using System.Linq;
using Ecs.Data;
using Inventory;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Object = UnityEngine.Object;

namespace Ecs
{
    public sealed class ChestInitSystem : IEcsInitSystem
    {
        //TODO move to static data
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
            _chestInventory = Object.Instantiate(_staticData.chestInventoryPrefab, Constants.buttonsPanel);
            
            var clickedChestSlotEntity = _world.NewEntity();
            var chestSlotMetaData = new SlotMetaData()
            {
                isHandled = true
            };
            clickedChestSlotEntity.Get<ChestTag>();
            clickedChestSlotEntity.Get<LatestClickedSlotComponent>() = new LatestClickedSlotComponent()
            {
                slotMetaData = chestSlotMetaData
            };

            for (int i = 0; i < 9; i++)
            {
                var slotButton = _chestInventory.content.GetChild(i).GetComponent<Button>();
                var i1 = i;
                slotButton.onClick.AddListener(delegate
                {
                    chestSlotMetaData.index = (byte)i1;
                    chestSlotMetaData.isHandled = false;
                });
            }
            
            var parentChest = Object.Instantiate(_staticData.parentChest);
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
                CheckForCollision(chest);
                
                var chestEntity = _world.NewEntity();
                chestEntity.Get<ChestTag>();
                chestEntity.Get<InteractableTag>();
                chestEntity.Get<InventoryTag>();
                chestEntity.Get<InteractableComponent>() = new InteractableComponent()
                {
                    collider = chest.GetComponent<BoxCollider>(),
                    transform = chest.transform,
                    type = InteractableType.Chest
                };

                chestEntity.Get<ScrollViewComponent>() = new ScrollViewComponent()
                {
                    scrollView = _chestInventory
                };

                var inventoryComponent = new InventoryComponent
                {
                    slotComponents = new List<SlotComponent>(_staticData.inventoryCapacity)
                };
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

            _chestInventory.GetComponentsInChildren<Button>().First(button => button.CompareTag("CloseButton")).onClick.AddListener(CloseChestInventory);
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

            slotComponent.itemSprite = _staticData.GetSpriteByItemType(slotComponent.itemComponent?.item.itemType);

            return slotComponent;
        }

        private Item GenerateRandomItem()
        {
            var itemType = Randomizer.GetRandomEnumValue<ItemType>();
            var item = new Item
            {
                itemType = itemType,
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
        
        private void CheckForCollision(GameObject gameObject)
        {
            var attempts = 0;           

            while (attempts++ < 100)
            {         
                var interactableFilter = (EcsFilter<InteractableTag, InteractableComponent>)_world.GetFilter(typeof(EcsFilter<InteractableTag, InteractableComponent>));

                var hasCollision = false;
                foreach (var i in interactableFilter)
                {
                    ref var interactableComponent = ref interactableFilter.Get2(i);
                    if (!interactableComponent.collider.bounds.Intersects(gameObject.GetComponent<Collider>().bounds))
                        continue;
                    hasCollision = true;
                    break;
                }

                if (!hasCollision)
                {
                    return;
                }

                gameObject.transform.position = new Vector3()
                {
                    x = Randomizer.GetRandomInRange(MinX, MaxX),
                    z = Randomizer.GetRandomInRange(MinZ, MaxZ),
                    y = 0.3f
                };
            }
            Debug.Log("couldn't fix collision");
        }
    }
}