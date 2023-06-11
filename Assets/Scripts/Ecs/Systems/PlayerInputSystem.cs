using Leopotam.Ecs;
using UnityEngine;

namespace Ecs
{
    public sealed class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerTag, DirectionComponent> directionFilter = null;
        private EcsFilter<MovingMetaDataComponent> _movingFilter;

        private float _moveX;
        private float _moveZ;
        private Vector2 _touchStartPosition;
        private bool _isWalking;

        public void Run()
        {
            SetDirection();
            foreach (var i in directionFilter)
            {
                ref var directionComponent = ref directionFilter.Get2(i);
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

                for (int j = 0; j < Input.touchCount; j++)
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
                    }

                    movingMetaDataComponent.isWalking = _isWalking;
                }
            }
        }
    }
}