using System.Collections.Generic;
using System.Linq;
using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Inventory;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ecs.Systems
{
    public sealed class ChestInitSystem : IEcsInitSystem
    {
        //TODO move to static data
        private const float MinX = -50;
        private const float MaxX = 50;
        private const float MinZ = -50;
        private const float MaxZ = 50;

        private ScrollRect _chestInventory;

        private EcsFilter<InventoryTag, ButtonComponent, ScrollViewComponent> _inventoryFilter;
        private Button _openChestButton;

        private StaticData _staticData;
        private EcsWorld _world;

        public void Init()
        {
            _chestInventory = Object.Instantiate(_staticData.chestInventoryPrefab, Constants.ButtonsPanel);

            var clickedChestSlotEntity = _world.NewEntity();
            var chestSlotMetaData = new SlotMetaData
            {
                IsHandled = true
            };
            clickedChestSlotEntity.Get<ChestTag>();
            clickedChestSlotEntity.Get<LatestClickedSlotComponent>() = new LatestClickedSlotComponent
            {
                SlotMetaData = chestSlotMetaData
            };

            for (var i = 0; i < 9; i++)
            {
                var slotButton = _chestInventory.content.GetChild(i).GetComponent<Button>();
                var i1 = i;
                slotButton.onClick.AddListener(delegate
                {
                    chestSlotMetaData.Index = (byte)i1;
                    chestSlotMetaData.IsHandled = false;
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
                chestEntity.Get<InteractableComponent>() = new InteractableComponent
                {
                    collider = chest.GetComponent<BoxCollider>(),
                    transform = chest.transform,
                    type = InteractableType.Chest
                };

                chestEntity.Get<ScrollViewComponent>() = new ScrollViewComponent
                {
                    ScrollView = _chestInventory
                };

                var inventoryComponent = new InventoryComponent
                {
                    SlotComponents = new List<SlotComponent>(_staticData.inventoryCapacity)
                };
                for (var j = 0; j < _staticData.inventoryCapacity; j++)
                    inventoryComponent.SlotComponents.Add(GenerateRandomSlot());

                chestEntity.Get<InventoryComponent>() = inventoryComponent;
            }

            var chestButtonEntity = _world.NewEntity();

            _openChestButton = Object.Instantiate(_staticData.openChestButtonPrefab, Constants.ButtonsPanel);
            _openChestButton.onClick.AddListener(OpenChestInventory);
            chestButtonEntity.Get<ButtonComponent>() = new ButtonComponent
            {
                Button = _openChestButton
            };
            chestButtonEntity.Get<OpenChestButtonTag>();

            _chestInventory.GetComponentsInChildren<Button>().First(button => button.CompareTag("CloseButton")).onClick
                .AddListener(CloseChestInventory);
        }

        private SlotComponent GenerateRandomSlot()
        {
            var slotComponent = new SlotComponent();

            if (Randomizer.GetRandomBool())
            {
                var itemComponent = new ItemComponent
                {
                    Item = GenerateRandomItem(),
                    Quantity = (byte)Randomizer.GetRandomInRange(1, 5)
                };
                if (itemComponent.Item.ItemType == ItemType.Weapon) itemComponent.Quantity = 1;

                slotComponent.ItemComponent = itemComponent;
            }

            slotComponent.ItemSprite = _staticData.GetSpriteByItemType(slotComponent.ItemComponent?.Item.ItemType);

            return slotComponent;
        }

        private static Item GenerateRandomItem()
        {
            var itemType = Randomizer.GetRandomEnumValue<ItemType>();
            var item = new Item
            {
                ItemType = itemType
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
                inventoryButton.Button.gameObject.SetActive(false);
                var inventoryScrollView = _inventoryFilter.Get3(i);
                inventoryScrollView.ScrollView.gameObject.SetActive(true);
            }

            var jumpButtonFilter =
                (EcsFilter<JumpTag, ButtonComponent>)_world.GetFilter(typeof(EcsFilter<JumpTag, ButtonComponent>));
            ToggleButton(jumpButtonFilter, false);
            var shootButtonFilter =
                (EcsFilter<ShootingButtonTag, ButtonComponent>)_world.GetFilter(
                    typeof(EcsFilter<ShootingButtonTag, ButtonComponent>));
            ToggleButton(shootButtonFilter, false);
        }

        private void CloseChestInventory()
        {
            Time.timeScale = 1;
            _chestInventory.gameObject.SetActive(false);
            _openChestButton.gameObject.SetActive(true);
            foreach (var i in _inventoryFilter)
            {
                var inventoryButton = _inventoryFilter.Get2(i);
                inventoryButton.Button.gameObject.SetActive(true);
                var inventoryScrollView = _inventoryFilter.Get3(i);
                inventoryScrollView.ScrollView.gameObject.SetActive(false);
            }

            var jumpButtonFilter =
                (EcsFilter<JumpTag, ButtonComponent>)_world.GetFilter(typeof(EcsFilter<JumpTag, ButtonComponent>));
            ToggleButton(jumpButtonFilter, true);
            var shootButtonFilter =
                (EcsFilter<ShootingButtonTag, ButtonComponent>)_world.GetFilter(
                    typeof(EcsFilter<ShootingButtonTag, ButtonComponent>));
            ToggleButton(shootButtonFilter, true);
        }

        private void ToggleButton<T>(EcsFilter<T, ButtonComponent> buttonFilter, bool active) where T : struct
        {
            foreach (var i in buttonFilter)
            {
                ref var buttonComponent = ref buttonFilter.Get2(i);
                buttonComponent.Button.gameObject.SetActive(active);
            }
        }

        private void CheckForCollision(GameObject gameObject)
        {
            var attempts = 0;

            while (attempts++ < 100)
            {
                var interactableFilter =
                    (EcsFilter<InteractableTag, InteractableComponent>)_world.GetFilter(
                        typeof(EcsFilter<InteractableTag, InteractableComponent>));

                var hasCollision = false;
                foreach (var i in interactableFilter)
                {
                    ref var interactableComponent = ref interactableFilter.Get2(i);
                    if (!interactableComponent.collider.bounds.Intersects(gameObject.GetComponent<Collider>().bounds))
                        continue;
                    hasCollision = true;
                    break;
                }

                if (!hasCollision) return;

                gameObject.transform.position = new Vector3
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