using System.Collections.Generic;
using System.Linq;
using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ecs.Systems
{
    public sealed class InventoryInitSystem : IEcsInitSystem
    {
        private Button _inventoryButton;
        private ScrollRect _inventoryScrollView;
        private StaticData _staticData;
        private EcsWorld _world;


        public void Init()
        {
            _inventoryScrollView = Object.Instantiate(_staticData.inventoryScrollViewPrefab, Constants.ButtonsPanel);

            var clickedPlayerInventorySlotEntity = _world.NewEntity();
            var chestSlotMetaData = new SlotMetaData
            {
                IsHandled = true
            };
            clickedPlayerInventorySlotEntity.Get<InventoryTag>();
            clickedPlayerInventorySlotEntity.Get<LatestClickedSlotComponent>() = new LatestClickedSlotComponent
            {
                SlotMetaData = chestSlotMetaData
            };

            for (var i = 0; i < 9; i++)
            {
                var slotButton = _inventoryScrollView.content.GetChild(i).GetComponent<Button>();
                var i1 = i;
                slotButton.onClick.AddListener(delegate
                {
                    chestSlotMetaData.Index = (byte)i1;
                    chestSlotMetaData.IsHandled = false;
                });
            }

            var inventoryEntity = _world.NewEntity();
            inventoryEntity.Get<PlayerTag>();
            inventoryEntity.Get<InventoryTag>();
            var inventoryComponent = new InventoryComponent
            {
                SlotComponents = new List<SlotComponent>(_staticData.inventoryCapacity)
            };


            for (var i = 0; i < _staticData.inventoryCapacity; i++)
                inventoryComponent.SlotComponents.Add(GenerateEmptySlot());

            var inventoryButton = Object.Instantiate(_staticData.inventoryButtonPrefab, Constants.ButtonsPanel);
            inventoryEntity.Get<ButtonComponent>() = new ButtonComponent
            {
                Button = inventoryButton
            };
            _inventoryButton = inventoryEntity.Get<ButtonComponent>().Button;

            var closeInventoryButton = _inventoryScrollView.GetComponentsInChildren<Button>()
                .First(button => button.CompareTag("CloseButton"));
            inventoryEntity.Get<ScrollViewComponent>() = new ScrollViewComponent
            {
                ScrollView = _inventoryScrollView
            };
            _inventoryScrollView = inventoryEntity.Get<ScrollViewComponent>().ScrollView;

            inventoryButton.onClick.AddListener(OnClickEvent);
            closeInventoryButton.onClick.AddListener(OnCloseEvent);

            inventoryEntity.Get<InventoryComponent>() = inventoryComponent;
        }

        private SlotComponent GenerateEmptySlot()
        {
            return new SlotComponent
            {
                ItemSprite = _staticData.emptyImage
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
                (EcsFilter<ShootingButtonTag, ButtonComponent>)_world.GetFilter(
                    typeof(EcsFilter<ShootingButtonTag, ButtonComponent>));
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
                (EcsFilter<ShootingButtonTag, ButtonComponent>)_world.GetFilter(
                    typeof(EcsFilter<ShootingButtonTag, ButtonComponent>));
            ToggleButton(shootButtonFilter, false);
        }

        private void ToggleButton<T>(EcsFilter<T, ButtonComponent> buttonFilter, bool active) where T : struct
        {
            foreach (var i in buttonFilter)
            {
                ref var buttonComponent = ref buttonFilter.Get2(i);
                buttonComponent.Button.gameObject.SetActive(active);
            }
        }
    }
}