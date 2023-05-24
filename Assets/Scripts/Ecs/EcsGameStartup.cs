using System;
using System.Collections;
using System.Collections.Generic;
using Ecs;
using Ecs.Data;
using Ecs.Systems;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

public sealed class EcsGameStartup : MonoBehaviour
{
    private EcsWorld world;
    private EcsSystems systems;
    public StaticData configuration;
    public SceneData sceneData;
    
    // Start is called before the first frame update
    void Start()
    {
        world = new EcsWorld();
        systems = new EcsSystems(world);
        
        systems.ConvertScene();
        
        AddOneFrames();
        AddSystems();
        AddInjections();

        systems.Init();
    }

    private void AddInjections()
    {
        var runtimeData = new RuntimeData();

        systems
            .Inject(configuration)
            .Inject(sceneData)
            .Inject(runtimeData)
            ;
    }

    private void AddOneFrames()
    {
        
    }

    private void AddSystems()
    {
        systems
            .Add(new PlayerInitSystem())
            .Add(new InventoryInitSystem())
            .Add(new InventoryButtonInitSystem())
            .Add(new ChestInitSystem())
            .Add(new PlayerInputSystem())
            .Add(new MovementSystem())
            .Add(new PlayerWeaponChaseSystem())
            .Add(new PlayerRightInputSystem())
            .Add(new PlayerLookSystem())
            .Add(new CheckInteractableSystem())
            .Add(new ToggleButtonSystem())
            ;
    }

    // Update is called once per frame
    void Update()
    {
        systems.Run();
    }

    private void OnDestroy()
    {
        if (systems == null)
        {
            return;
        }
        
        systems.Destroy();
        systems = null;
        
        world.Destroy();
        world = null;
    }
}