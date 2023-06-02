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
                
                speedBarComponent.fullBarValue -= speedBarComponent.decreaseAmount * Time.deltaTime;
                speedBarComponent.fullBarValue = Mathf.Clamp(speedBarComponent.fullBarValue, 0f, 1f);

                speedBarComponent.speedBarImage.fillAmount = speedBarComponent.fullBarValue;
            }
        }
    }
}