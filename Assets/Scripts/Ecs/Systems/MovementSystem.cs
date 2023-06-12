using Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class MovementSystem : IEcsRunSystem
    {
        private const float BobFrequency = 5f;
        private const float BobHorizontalAmplitude = 0.1f;
        private const float BobVerticalAmplitude = 0.1f;
        private const float HeadSmoothing = 0.1f;

        private EcsFilter<ModelComponent, MovableComponent, DirectionComponent, MovingMetaDataComponent> _movableFilter;
        private EcsFilter<SpeedBarComponent> _speedBarFilter;

        private Vector3 _targetCameraPosition;
        private float _walkingTime;

        public void Run()
        {
            foreach (var i in _movableFilter)
            {
                ref var modelComponent = ref _movableFilter.Get1(i);
                ref var movableComponent = ref _movableFilter.Get2(i);
                ref var directionComponent = ref _movableFilter.Get3(i);
                ref var movingMetaDataComponent = ref _movableFilter.Get4(i);

                ref var direction = ref directionComponent.direction;
                ref var transform = ref modelComponent.modelTransform;

                ref var characterController = ref movableComponent.characterController;
                ref var speed = ref movableComponent.speed;

                foreach (var j in _speedBarFilter)
                {
                    ref var speedBarComponent = ref _speedBarFilter.Get1(j);

                    speed = speedBarComponent.FullBarValue switch
                    {
                        0 => 5,
                        1 => 10,
                        _ => speed
                    };
                }

                var rawDirection = direction.x * transform.right + direction.z * transform.forward;
                rawDirection.y -=
                    Mathf.Sqrt(5f * -3.0f * -9.8f); // jumpforce, const, gravity TODO: extract from filter
                var normalizedDirection = rawDirection.normalized;
                characterController.Move(normalizedDirection * speed * Time.deltaTime);

                //Set time and offset to 0
                if (!movingMetaDataComponent.isWalking)
                    _walkingTime = 0;
                else
                    _walkingTime += Time.deltaTime;

                if (normalizedDirection.sqrMagnitude <= 0.001)
                {
                    movingMetaDataComponent.isWalking = false;
                    return;
                }

                //Calculate the camera's target position
                _targetCameraPosition = transform.position + CalculateHeadBobOffset(_walkingTime);

                //Interpolate position
                Camera.main!.transform.position = Vector3.Lerp(Camera.main.transform.position, _targetCameraPosition,
                    HeadSmoothing);

                //Snap to position if it is close enough
                if ((Camera.main.transform.position - _targetCameraPosition).magnitude <= 0.001)
                    Camera.main.transform.position = _targetCameraPosition;
            }
        }

        private Vector3 CalculateHeadBobOffset(float t)
        {
            var offset = Vector3.zero;

            if (!(t > 0)) return offset;

            //Calculate offsets
            var horizontalOffSet = Mathf.Cos(t * BobFrequency) * BobHorizontalAmplitude;
            var verticalOffset = Mathf.Sin(t * BobFrequency * 2) * BobVerticalAmplitude;

            //Combine offsets relative to the head's position and calculate the camera's target position
            foreach (var i in _movableFilter)
            {
                var modelComponent = _movableFilter.Get1(i);
                offset = modelComponent.modelTransform.right * horizontalOffSet +
                         modelComponent.modelTransform.up * verticalOffset;
            }

            return offset;
        }
    }
}