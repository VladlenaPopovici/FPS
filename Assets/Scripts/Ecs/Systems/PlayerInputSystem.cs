using System;
using Ecs.Components;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class PlayerInputSystem : IEcsRunSystem
    {
        private EcsFilter<PlayerTag, DirectionComponent> _directionFilter;
        private bool _isWalking;

        private float _moveX;
        private float _moveZ;
        private EcsFilter<MovingMetaDataComponent> _movingFilter;
        private Vector2 _touchStartPosition;

        public void Run()
        {
            SetDirection();
            foreach (var i in _directionFilter)
            {
                ref var directionComponent = ref _directionFilter.Get2(i);
                ref var direction = ref directionComponent.direction;

                direction.x = _moveX;
                direction.z = _moveZ;
            }
        }

        private void SetDirection()
        {
            foreach (var i in _movingFilter)
            {
                ref var movingMetaDataComponent = ref _movingFilter.Get1(i);

                if (Input.touchCount <= 0) return;

                for (var j = 0; j < Input.touchCount; j++)
                {
                    var touch = Input.GetTouch(j);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            _touchStartPosition = touch.position;
                            _isWalking = true;
                            break;
                        case TouchPhase.Moved:
                            var touchDelta = touch.position - _touchStartPosition;
                            if (touch.position.x < Screen.width / 2)
                            {
                                _moveX = touchDelta.x;
                                _moveZ = touchDelta.y;
                            }
                            else
                            {
                                _moveX = 0f;
                                _moveZ = 0f;
                            }

                            _isWalking = true;
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            // Reset movement values
                            _moveX = 0f;
                            _moveZ = 0f;
                            _isWalking = false;
                            break;
                        case TouchPhase.Stationary:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    movingMetaDataComponent.isWalking = _isWalking;
                }
            }
        }
    }
}