using Leopotam.Ecs;
using UnityEngine;

namespace Ecs
{
    public sealed class PlayerLookSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter<PlayerTag> playerFilter = null;
        private readonly EcsFilter<PlayerTag, ModelComponent, LookDirectionComponent> lookFilter = null;

        private Quaternion startTransformRotation;

        public void Init()
        {
            foreach (var i in playerFilter)
            {
                ref var playerEntity = ref playerFilter.GetEntity(0);
                ref var model = ref playerEntity.Get<ModelComponent>();

                startTransformRotation = model.modelTransform.rotation;
            }
        }

        public void Run()
        {
            foreach (var i in lookFilter)
            {
                ref var model = ref lookFilter.Get2(i);
                ref var lookComponent = ref lookFilter.Get3(i);

                var axisX = lookComponent.direction.x;
                var axisY = lookComponent.direction.y;
                
                var rotateX = 
                    Quaternion.AngleAxis(axisX, Vector3.up * Time.deltaTime * lookComponent.mouseSensitivity);
                var rotateY = 
                    Quaternion.AngleAxis(axisY, Vector3.right * Time.deltaTime * lookComponent.mouseSensitivity);
                
                model.modelTransform.rotation = startTransformRotation * rotateX;
                lookComponent.cameraTransform.rotation = model.modelTransform.rotation * rotateY;
            }
        }
    }
}