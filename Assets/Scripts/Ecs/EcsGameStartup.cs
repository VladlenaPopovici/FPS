using System;
using System.Collections;
using System.Collections.Generic;
using Ecs;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

public sealed class EcsGameStartup : MonoBehaviour
{
    private EcsWorld world;
    private EcsSystems systems;
    
    // Start is called before the first frame update
    void Start()
    {
        world = new EcsWorld();
        systems = new EcsSystems(world);

        systems.ConvertScene();
        
        AddInjections();
        AddOneFrames();
        AddSystems();

        systems.Init();
    }

    private void AddInjections()
    {
        
    }

    private void AddOneFrames()
    {
        
    }

    private void AddSystems()
    {
        systems
            .Add(new PlayerInputSystem())
            .Add(new MovementSystem());
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
