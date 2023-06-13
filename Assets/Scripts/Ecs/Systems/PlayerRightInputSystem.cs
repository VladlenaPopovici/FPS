using System;
using Ecs.Components;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class PlayerRightInputSystem : IEcsRunSystem
    {
        private float _axisX;
        private float _axisY;
        private EcsFilter<PlayerTag, LookDirectionComponent> _lookFilter;
        private Vector2 _touchStartPosition;

        public void Run()
        {
            GetAxis();
            ClampAxis();

            foreach (var i in _lookFilter)
            {
                ref var lookDirection = ref _lookFilter.Get2(i);
                ref var direction = ref lookDirection.direction;

                direction.x = _axisX;
                direction.y = _axisY;
            }
        }

        private void ClampAxis()
        {
            _axisY = Mathf.Clamp(_axisY, -90, 90);
        }

        private void GetAxis()
        {
            if (Input.touchCount <= 0) return;

            for (var i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touchStartPosition = touch.position;
                        break;
                    case TouchPhase.Moved:
                        var touchDelta = touch.position - _touchStartPosition;
                        if (touch.position.x > Screen.width / 2)
                        {
                            _axisX = touchDelta.x;
                            _axisY = touchDelta.y;
                        }

                        break;
                    case TouchPhase.Stationary:
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}