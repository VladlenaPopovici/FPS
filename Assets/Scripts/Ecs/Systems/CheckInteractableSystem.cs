using System.Collections.Generic;
using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public class CheckInteractableSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter<PlayerTag, ModelComponent, LookDirectionComponent> _playerFilter;
        private EcsFilter<InteractableTag, InteractableComponent> _interactableFilter;
        private EcsFilter<OpenChestButtonTag, ButtonComponent> _chestButtonFilter;
        private StaticData _staticData;
        
        private readonly List<InteractableMetadata> _interactableMetaData = new();

        public void Init()
        {
            foreach (var i in _interactableFilter)
            {
                var interactableComponent = _interactableFilter.Get2(i);
                _interactableMetaData.Add(
                    new InteractableMetadata()
                    {
                        Position = interactableComponent.transform.position,
                        Collider = interactableComponent.collider,
                        Type = interactableComponent.type
                    }
                );
            }
        }

        public void Run()
        {
            var playerPosition = Vector3.zero;
            Transform cameraTransform = null;

            foreach (var i in _playerFilter)
            {
                playerPosition = _playerFilter.Get2(i).modelTransform.position;
                cameraTransform = _playerFilter.Get3(i).cameraTransform;
            }

            var ray = new Ray(cameraTransform.position, cameraTransform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 3);

            var isChestNear = false;
            foreach (var interactable in _interactableMetaData)
            {
                if (IsNear(playerPosition, interactable.Position) && IsLooking(ray, interactable.Collider))
                {
                    if (interactable.Type == InteractableType.Chest)
                    {
                        isChestNear = true;
                    }
                }
            }

            foreach (var i in _chestButtonFilter)
            {
                ref var chestButtonComponent = ref _chestButtonFilter.Get2(i);
                chestButtonComponent.isVisible = isChestNear;
                chestButtonComponent.button.gameObject.SetActive(isChestNear);
            }
        }

        private bool IsLooking(Ray ray, Collider interactableValue)
        {
            if (!Physics.Raycast(ray, out var hitInfo, 3, _staticData.layerMask)) return false;

            return hitInfo.collider == interactableValue;
        }

        private static bool IsNear(Vector3 playerPosition, Vector3 interactableComponentPosition)
        {
            float radius = 3;
            var distance = Vector3.Distance(playerPosition, interactableComponentPosition);

            return distance < radius;
        }
    }

    public struct InteractableMetadata
    {
        public Vector3 Position;
        public Collider Collider;
        public InteractableType Type;
    }
}