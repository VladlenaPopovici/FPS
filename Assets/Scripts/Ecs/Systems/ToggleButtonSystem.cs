using Ecs;
using Leopotam.Ecs;

public sealed class ToggleButtonSystem : IEcsRunSystem
{
    private EcsFilter<ButtonComponent> buttonsFilter;

    public void Run()
    {
        foreach (var i in buttonsFilter)
        {
            var buttonComponent = buttonsFilter.Get1(i);
            buttonComponent.button.gameObject.SetActive(buttonComponent.isVisible);
        }
    }
}