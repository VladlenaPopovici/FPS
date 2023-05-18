using Leopotam.Ecs;
using UnityEngine;

namespace Ecs
{
    public sealed class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerTag, DirectionComponent> directionFilter = null;

        private float _moveX;
        private float _moveZ;
        private Vector2 _touchStartPosition;

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
            if (Input.touchCount <= 0) return;

            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touchStartPosition = touch.position;
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

                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        // Reset movement values
                        _moveX = 0f;
                        _moveZ = 0f;
                        break;
                }
            }
        }
    }
}