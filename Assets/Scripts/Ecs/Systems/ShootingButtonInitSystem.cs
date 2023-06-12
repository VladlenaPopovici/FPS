using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace Ecs.Systems
{
    public sealed class ShootingButtonInitSystem : IEcsInitSystem
    {
        private StaticData _staticData;
        private EcsWorld _world;

        public void Init()
        {
            var bulletParent = Object.Instantiate(_staticData.bulletParentPrefab);
            _staticData.bulletParent = bulletParent;

            var button = Object.Instantiate(_staticData.shootButtonPrefab, Constants.ButtonsPanel);
            var eventTrigger = button.GetComponent<EventTrigger>();

            // Create the tap event trigger
            CreateTapTrigger(eventTrigger);

            // Create the release event trigger
            CreateReleaseTrigger(eventTrigger);

            var shootButtonEntity = _world.NewEntity();
            shootButtonEntity.Get<ShootingButtonTag>();
            shootButtonEntity.Get<ButtonComponent>() = new ButtonComponent
            {
                Button = button
            };
            shootButtonEntity.Get<ButtonHoldComponent>() = new ButtonHoldComponent();
        }

        private void CreateReleaseTrigger(EventTrigger eventTrigger)
        {
            var releaseEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
            releaseEntry.callback.AddListener(_ => OnHoldButtonReleased());
            eventTrigger.triggers.Add(releaseEntry);
        }

        private void CreateTapTrigger(EventTrigger eventTrigger)
        {
            var tapEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            tapEntry.callback.AddListener(_ => OnHoldButtonClicked());
            eventTrigger.triggers.Add(tapEntry);
        }

        private void OnHoldButtonClicked()
        {
            var buttonHoldFilter =
                (EcsFilter<ButtonHoldComponent>)_world.GetFilter(typeof(EcsFilter<ButtonHoldComponent>));
            foreach (var i in buttonHoldFilter)
            {
                ref var buttonHold = ref buttonHoldFilter.Get1(i);
                buttonHold.IsButtonHeld = true;
                buttonHold.IsButtonReleased = false;
                buttonHold.HoldTimer = 0f;
            }
        }

        private void OnHoldButtonReleased()
        {
            var buttonHoldFilter =
                (EcsFilter<ButtonHoldComponent>)_world.GetFilter(typeof(EcsFilter<ButtonHoldComponent>));
            foreach (var i in buttonHoldFilter)
            {
                ref var buttonHold = ref buttonHoldFilter.Get1(i);
                buttonHold.IsButtonHeld = false;
                buttonHold.IsButtonReleased = true;
            }
        }
    }
}