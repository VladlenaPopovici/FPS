using System.Globalization;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class HealthBarSystem : IEcsRunSystem
    {
        private EcsFilter<PlayerTag, HealthBarComponent> _healthBarFilter;
        
        public void Run()
        {
            foreach (var i in _healthBarFilter)
            {
                ref var healthBarComponent = ref _healthBarFilter.Get2(i);

                var fillAmount = healthBarComponent.hp / 100;
                healthBarComponent.hpBar.fillAmount = fillAmount;

                healthBarComponent.hpBar.color = Color.Lerp(Color.red, Color.green, fillAmount);

                healthBarComponent.hpBar.GetComponentInChildren<TextMeshProUGUI>().text =
                    healthBarComponent.hp.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}