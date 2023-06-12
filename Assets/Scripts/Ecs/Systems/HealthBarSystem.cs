using System.Globalization;
using Ecs.Tags;
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

                var fillAmount = healthBarComponent.Hp / 100;
                healthBarComponent.HpBar.fillAmount = fillAmount;

                healthBarComponent.HpBar.color = Color.Lerp(Color.red, Color.green, fillAmount);

                healthBarComponent.HpBar.GetComponentInChildren<TextMeshProUGUI>().text =
                    healthBarComponent.Hp.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}