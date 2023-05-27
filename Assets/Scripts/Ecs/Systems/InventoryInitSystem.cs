using System.Collections.Generic;
using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ecs
{
    public sealed class InventoryInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private StaticData _staticData;
        private ScrollRect _inventoryScrollView;
        private Button _inventoryButton;


        public void Init()
        {
            var inventoryEntity = _world.NewEntity();
            inventoryEntity.Get<InventoryTag>();
            var inventoryComponent = new InventoryComponent();
            inventoryComponent.slotComponents = new List<SlotComponent>(_staticData.inventoryCapacity);

            for (int i = 0; i < _staticData.inventoryCapacity; i++)
            {
                inventoryComponent.slotComponents.Add(new SlotComponent());
            }

            var inventoryButton = Object.Instantiate(_staticData.inventoryButtonPrefab, Constants.buttonsPanel);
            inventoryEntity.Get<ButtonComponent>() = new ButtonComponent()
            {
                button = inventoryButton,
                isVisible = true
            };
            _inventoryButton = inventoryEntity.Get<ButtonComponent>().button;

            var inventoryScrollView = Object.Instantiate(_staticData.inventoryScrollViewPrefab, Constants.buttonsPanel);
            var closeInventoryButton = inventoryScrollView.GetComponentInChildren<Button>();
            inventoryEntity.Get<ScrollViewComponent>() = new ScrollViewComponent()
            {
                scrollView = inventoryScrollView,
                closeButton = closeInventoryButton
            };
            _inventoryScrollView = inventoryEntity.Get<ScrollViewComponent>().scrollView;
            
            inventoryButton.onClick.AddListener(OnClickEvent);
            closeInventoryButton.onClick.AddListener(OnCloseEvent);

            inventoryEntity.Get<InventoryComponent>() = inventoryComponent;
        }

        private void OnCloseEvent()
        {
            Time.timeScale = 1;
            _inventoryScrollView.gameObject.SetActive(false);
            _inventoryButton.gameObject.SetActive(true);
        }

        private void OnClickEvent()
        {
            Time.timeScale = 0;
            _inventoryButton.gameObject.SetActive(false);
            _inventoryScrollView.gameObject.SetActive(true);
        }
    }
}