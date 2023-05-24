using System.Collections.Generic;
using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ecs
{
    public sealed class ChestInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;
        private StaticData _staticData;
        private const float minX = -50;
        private const float maxX = 50;
        private const float minZ = -50;
        private const float maxZ = 50;
        private ScrollRect chestInventory;
        private Button openChestButton;


        public void Init()
        {
            var parentChest = Object.Instantiate(_staticData.parentChest);
            chestInventory = Object.Instantiate(_staticData.chestInventoryPrefab, Constants.buttonsPanel);

            for (var i = 0; i < 10; i++)
            {
                var position = new Vector3
                {
                    x = Randomizer.GetRandomInRange(minX, maxX),
                    z = Randomizer.GetRandomInRange(minZ, maxZ),
                    y = 0.3f
                };

                var rotationVector = new Vector3
                {
                    y = Randomizer.GetRandomInRange(Constants.MinAngle, Constants.MaxAngle)
                };
                var rotation = Quaternion.Euler(rotationVector);


                var chest = Object.Instantiate(_staticData.chestPrefab, position, rotation, parentChest.transform);

                var chestEntity = _world.NewEntity();
                chestEntity.Get<InteractableComponent>() = new InteractableComponent()
                {
                    collider = chest.GetComponent<Collider>(),
                    transform = chest.transform,
                };
                chestEntity.Get<InteractableTag>();
            
                chestEntity.Get<InventoryTag>();
                InventoryComponent inventoryComponent = chestEntity.Get<InventoryComponent>();
                inventoryComponent.slotComponents = new List<SlotComponent>(_staticData.inventoryCapacity);
                inventoryComponent.inventoryScrollView = chestInventory;

                for (int j = 0; j < _staticData.inventoryCapacity; j++)
                {
                    inventoryComponent.slotComponents.Add(new SlotComponent());
                }
            }
            
            
            var chestButtonEntity = _world.NewEntity();
            
            openChestButton = Object.Instantiate(_staticData.openChestButtonPrefab, Constants.buttonsPanel);
            openChestButton.onClick.AddListener(OpenChestInventory);
            chestButtonEntity.Get<ChestButtonComponent>() = new ChestButtonComponent()
            {
                button = openChestButton
            };
            chestButtonEntity.Get<OpenChestButtonTag>();

            chestInventory.GetComponentInChildren<Button>().onClick.AddListener(CloseChestInventory);
        }

        private void OpenChestInventory()
        {
            chestInventory.gameObject.SetActive(true);
        }

        private void CloseChestInventory()
        {
            chestInventory.gameObject.SetActive(false);
        }
    }
}