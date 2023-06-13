using Ecs.Components;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class PlayerLookSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter<PlayerTag, ModelComponent, LookDirectionComponent> _lookFilter = null;
        private readonly EcsFilter<PlayerComponent> _playerFilter = null;

        private Quaternion _startTransformRotation;

        public void Init()
        {
            foreach (var i in _playerFilter)
            {
                ref var playerEntity = ref _playerFilter.GetEntity(0);
                ref var model = ref playerEntity.Get<ModelComponent>();

                _startTransformRotation = model.modelTransform.rotation;
            }
        }

        public void Run()
        {
            foreach (var i in _lookFilter)
            {
                ref var model = ref _lookFilter.Get2(i);
                ref var lookComponent = ref _lookFilter.Get3(i);

                var axisX = lookComponent.direction.x;
                var axisY = lookComponent.direction.y;

                var rotateX =
                    Quaternion.AngleAxis(axisX, Vector3.up * Time.deltaTime * lookComponent.mouseSensitivity);
                var rotateY =
                    Quaternion.AngleAxis(axisY, Vector3.right * Time.deltaTime * lookComponent.mouseSensitivity);

                model.modelTransform.rotation = _startTransformRotation * rotateX;
                lookComponent.cameraTransform.rotation = model.modelTransform.rotation * rotateY;
            }
        }
    }
}