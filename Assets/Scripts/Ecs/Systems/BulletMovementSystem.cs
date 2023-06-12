using System;
using Ecs.Components;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class BulletMovementSystem : IEcsRunSystem
    {
        private EcsFilter<BulletComponent> _bulletFilter;
        private EcsFilter<PlayerTag, HealthBarComponent> _healthBarFilter;
        private EcsFilter<InteractableTag, InteractableComponent> _interactableFilter;

        private EcsWorld _world;

        public void Run()
        {
            foreach (var i in _bulletFilter)
            {
                ref var bulletComponent = ref _bulletFilter.Get1(i);

                if (OutOfScene(bulletComponent.GameObject.transform.position)) bulletComponent.IsDespawned = true;

                if (HasCollision(bulletComponent.GameObject.GetComponent<Collider>()))
                    bulletComponent.IsDespawned = true;
            }
        }

        private bool HasCollision(Collider bulletCollider)
        {
            foreach (var i in _interactableFilter)
            {
                var interactableComponent = _interactableFilter.Get2(i);

                if (interactableComponent.collider == null) continue; // TODO random NPE on first? chest collider
                if (!interactableComponent.collider.bounds.Intersects(bulletCollider.bounds))
                    continue;
                switch (interactableComponent.type)
                {
                    case InteractableType.Player:
                        DamagePlayer();
                        break;
                    case InteractableType.Enemy:
                        DamageEnemy();
                        break;
                    case InteractableType.Chest:
                    case InteractableType.Tree:
                    case InteractableType.Rock:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return true;
            }

            return false;
        }

        private void DamagePlayer()
        {
            foreach (var i in _healthBarFilter)
            {
                ref var healthBarComponent = ref _healthBarFilter.Get2(i);
                healthBarComponent.Hp -= 5;
            }
        }

        private static void DamageEnemy()
        {
            //TODO 
            Debug.Log("Bullet collision with enemy");
        }

        private static bool OutOfScene(Vector3 bulletPosition)
        {
            return bulletPosition.x is < -50 or > 50 ||
                   bulletPosition.z is < -50 or > 50 ||
                   bulletPosition.y is < 0 or > 10;
        }
    }
}