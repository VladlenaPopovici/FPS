using Leopotam.Ecs;
using UnityEngine;

namespace Ecs
{
    public sealed class PlayerRightInputSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerTag, LookDirectionComponent> lookFilter = null;
        private Vector2 _touchStartPosition;
        private float _axisX;
        private float _axisY;

        public void Run()
        {
            GetAxis();
            ClampAxis();

            foreach (var i in lookFilter)
            {
                ref var lookDirection = ref lookFilter.Get2(i);
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
                        if (touch.position.x > Screen.width / 2)
                        {
                            _axisX = touchDelta.x;
                            _axisY = touchDelta.y;
                        }

                        break;
                }
            }
        }
    }
}