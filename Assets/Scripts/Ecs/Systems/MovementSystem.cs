
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs
{
    public sealed class MovementSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ModelComponent, MovableComponent, DirectionComponent> movableFilter = null;

        public void Run()
        {
            foreach (var i in movableFilter)
            {
                ref var modelComponent = ref movableFilter.Get1(i);
                ref var movableComponent = ref movableFilter.Get2(i);
                ref var directionComponent = ref movableFilter.Get3(i);

                ref var direction = ref directionComponent.direction;
                ref var transform = ref modelComponent.modelTransform;

                ref var characterController = ref movableComponent.characterController;
                ref var speed = ref movableComponent.speed;

                var rawDirection = (direction.x * transform.right) + (direction.z * transform.forward);
                var normalizedDirection = rawDirection.normalized;
                characterController.Move(normalizedDirection * speed * Time.deltaTime);
            }
        }
    }
}