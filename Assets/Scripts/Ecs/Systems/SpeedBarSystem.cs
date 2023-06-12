using Ecs.Components;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class SpeedBarSystem : IEcsRunSystem
    {
        private EcsFilter<PlayerTag, SpeedBarComponent> _speedBarFilter;

        public void Run()
        {
            foreach (var i in _speedBarFilter)
            {
                ref var speedBarComponent = ref _speedBarFilter.Get2(i);

                speedBarComponent.FullBarValue -= speedBarComponent.DecreaseAmount * Time.deltaTime;
                speedBarComponent.FullBarValue = Mathf.Clamp(speedBarComponent.FullBarValue, 0f, 1f);

                speedBarComponent.SpeedBarImage.fillAmount = speedBarComponent.FullBarValue;
            }
        }
    }
}