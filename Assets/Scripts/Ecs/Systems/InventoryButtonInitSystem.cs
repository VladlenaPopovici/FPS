using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Ecs
{
    public sealed class InventoryButtonInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;
        private StaticData _staticData;
        private Button inventoryButton;
        private ScrollRect inventoryScrollView;

        public void Init()
        {
            EcsEntity inventoryEntity = _world.NewEntity();
            ref InventoryComponent inventoryComponent = ref inventoryEntity.Get<InventoryComponent>();
            var buttonComponent = inventoryEntity.Get<ButtonComponent>();
            inventoryComponent.inventoryButton = buttonComponent;

            var buttonsPanelObj = GameObject.FindWithTag("UI");
            var buttonsPanel = buttonsPanelObj.GetComponent<RectTransform>();
            inventoryComponent.inventoryButton.button = Object.Instantiate(_staticData.inventoryButtonPrefab, buttonsPanel);
            inventoryButton = inventoryComponent.inventoryButton.button;
            inventoryButton.onClick.AddListener(OnClickEvent);

            inventoryComponent.inventoryScrollView =
                Object.Instantiate(_staticData.inventoryScrollViewPrefab, buttonsPanel);
            inventoryScrollView = inventoryComponent.inventoryScrollView;
            inventoryComponent.closeInventoryButton = inventoryScrollView.GetComponentInChildren<Button>();
            inventoryComponent.closeInventoryButton.onClick.AddListener(OnCloseEvent);
        }

        private void OnCloseEvent()
        {
            Time.timeScale = 1;
            inventoryButton.gameObject.SetActive(true);
            inventoryScrollView.gameObject.SetActive(false);
        }

        private void OnClickEvent()
        {
            Time.timeScale = 0;
            inventoryButton.gameObject.SetActive(false);
            inventoryScrollView.gameObject.SetActive(true);
        }
    }
}