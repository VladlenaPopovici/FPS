using System.Collections.Generic;
using System.Linq;
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
            _inventoryScrollView = Object.Instantiate(_staticData.inventoryScrollViewPrefab, Constants.buttonsPanel);

            var clickedPlayerInventorySlotEntity = _world.NewEntity();
            var chestSlotMetaData = new SlotMetaData()
            {
                isHandled = true
            };
            clickedPlayerInventorySlotEntity.Get<InventoryTag>();
            clickedPlayerInventorySlotEntity.Get<LatestClickedSlotComponent>() = new LatestClickedSlotComponent()
            {
                slotMetaData = chestSlotMetaData
            };

            for (int i = 0; i < 9; i++)
            {
                var slotButton = _inventoryScrollView.content.GetChild(i).GetComponent<Button>();
                var i1 = i;
                slotButton.onClick.AddListener(delegate
                {
                    chestSlotMetaData.index = (byte)i1;
                    chestSlotMetaData.isHandled = false;
                });
            }

            var inventoryEntity = _world.NewEntity();
            inventoryEntity.Get<PlayerTag>();
            inventoryEntity.Get<InventoryTag>();
            var inventoryComponent = new InventoryComponent
            {
                slotComponents = new List<SlotComponent>(_staticData.inventoryCapacity)
            };


            for (int i = 0; i < _staticData.inventoryCapacity; i++)
            {
                inventoryComponent.slotComponents.Add(GenerateEmptySlot());
            }

            var inventoryButton = Object.Instantiate(_staticData.inventoryButtonPrefab, Constants.buttonsPanel);
            inventoryEntity.Get<ButtonComponent>() = new ButtonComponent()
            {
                button = inventoryButton,
                isVisible = true
            };
            _inventoryButton = inventoryEntity.Get<ButtonComponent>().button;

            var closeInventoryButton = _inventoryScrollView.GetComponentsInChildren<Button>()
                .First(button => button.CompareTag("CloseButton"));
            inventoryEntity.Get<ScrollViewComponent>() = new ScrollViewComponent()
            {
                scrollView = _inventoryScrollView,
                closeButton = closeInventoryButton
            };
            _inventoryScrollView = inventoryEntity.Get<ScrollViewComponent>().scrollView;

            inventoryButton.onClick.AddListener(OnClickEvent);
            closeInventoryButton.onClick.AddListener(OnCloseEvent);

            inventoryEntity.Get<InventoryComponent>() = inventoryComponent;
        }

        private SlotComponent GenerateEmptySlot()
        {
            return new SlotComponent()
            {
                itemSprite = _staticData.emptySprite
            };
        }

        private void OnCloseEvent()
        {
            Time.timeScale = 1;
            _inventoryScrollView.gameObject.SetActive(false);
            _inventoryButton.gameObject.SetActive(true);
            
        
            var jumpButtonFilter =
                (EcsFilter<JumpTag, ButtonComponent>)_world.GetFilter(typeof(EcsFilter<JumpTag, ButtonComponent>));
            ToggleButton(jumpButtonFilter, true);
            var shootButtonFilter =
                (EcsFilter<ShootingButtonTag, ButtonComponent>)_world.GetFilter(typeof(EcsFilter<ShootingButtonTag, ButtonComponent>));
            ToggleButton(shootButtonFilter, true);
        }

        private void OnClickEvent()
        {
            Time.timeScale = 0;
            _inventoryButton.gameObject.SetActive(false);
            _inventoryScrollView.gameObject.SetActive(true);
            
            var jumpButtonFilter =
                (EcsFilter<JumpTag, ButtonComponent>)_world.GetFilter(typeof(EcsFilter<JumpTag, ButtonComponent>));
            ToggleButton(jumpButtonFilter, false);
            var shootButtonFilter =
                (EcsFilter<ShootingButtonTag, ButtonComponent>)_world.GetFilter(typeof(EcsFilter<ShootingButtonTag, ButtonComponent>));
            ToggleButton(shootButtonFilter, false);
        }

        private void ToggleButton<T>(EcsFilter<T, ButtonComponent> buttonFilter, bool active) where T : struct
        {
            foreach (var i in buttonFilter)
            {
                ref var buttonComponent = ref buttonFilter.Get2(i);
                buttonComponent.button.gameObject.SetActive(active);
            }
        }
    }
}