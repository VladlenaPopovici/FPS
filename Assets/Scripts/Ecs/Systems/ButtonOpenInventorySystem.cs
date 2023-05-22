using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs
{
    public sealed class ButtonOpenInventorySystem : IEcsRunSystem
    {
        private readonly EcsFilter<ButtonComponent> buttonFilter = null;
        private StaticData _staticData;

        public void Run()
        {
            foreach (var i in buttonFilter)
            {
                ref var buttonComponent = ref buttonFilter.Get1(i);
            }
        }

    }
}