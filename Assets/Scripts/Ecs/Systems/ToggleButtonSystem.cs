using Ecs;
using Leopotam.Ecs;

public sealed class ToggleButtonSystem : IEcsRunSystem
{
    private EcsFilter<ChestButtonComponent> buttonsFilter;

    public void Run()
    {
        foreach (var i in buttonsFilter)
        {
            var chestButtonComponent = buttonsFilter.Get1(i);
            chestButtonComponent.button.gameObject.SetActive(chestButtonComponent.isVisible);
        }
    }
}