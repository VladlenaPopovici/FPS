using System.Collections.Generic;
using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public class CheckInteractableSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter<InteractableTag, InteractableComponent> interactableFilter = null;
        private readonly EcsFilter<PlayerTag, ModelComponent, LookDirectionComponent> playerFilter = null;
        private EcsFilter<OpenChestButtonTag, ButtonComponent> chestButtonFilter;
        private List<KeyValuePair<Vector3, Collider>> interactableMetaData = new();
        private StaticData _staticData;

        public void Init()
        {
            foreach (var i in interactableFilter)
            {
                var interactableComponent = interactableFilter.Get2(i);
                interactableMetaData.Add(
                    new KeyValuePair<Vector3, Collider>(interactableComponent.transform.position,
                        interactableComponent.collider)
                );
            }
        }

        public void Run()
        {
            var playerPosition = Vector3.zero;
            Transform cameraTransform = null;

            foreach (var i in playerFilter)
            {
                playerPosition = playerFilter.Get2(i).modelTransform.position;
                cameraTransform = playerFilter.Get3(i).cameraTransform;
            }

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 3);

            bool isAnyNear = false;
            foreach (var interactable in interactableMetaData)
            {
                if (isNear(playerPosition, interactable.Key) && isLooking(ray, interactable.Value))
                {
                    isAnyNear = true;
                }
            }
            
            foreach (var i in chestButtonFilter)
            {
                ref var chestButtonComponent = ref chestButtonFilter.Get2(i);
                chestButtonComponent.isVisible = isAnyNear;
            }
        }

        private bool isLooking(Ray ray, Collider interactableValue)
        {
            if (!Physics.Raycast(ray, out var hitInfo, 3, _staticData.layerMask)) return false;

            return hitInfo.collider == interactableValue;
        }

        private bool isNear(Vector3 playerPosition, Vector3 interactableComponentPosition)
        {
            float radius = 3;
            var distance = Vector3.Distance(playerPosition, interactableComponentPosition);
            return distance < radius;
        }
    }
}