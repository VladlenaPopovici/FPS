using System;
using System.Linq;
using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Ecs.Systems
{
    public class SceneInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private StaticData _staticData;

        public void Init()
        {
            var parentNature = Object.Instantiate(_staticData.parentNature);
            var parentTrees = ExtractChild(parentNature, "Trees");
            GenerateTrees(parentTrees);
            var parentRocks = ExtractChild(parentNature, "Rocks");
            GenerateRocks(parentRocks);
            var parentPlants = ExtractChild(parentNature, "Plants");
            GeneratePlants(parentPlants);
        }

        private static GameObject ExtractChild(GameObject parentNature, string childName)
        {
            return parentNature
                .GetComponentsInChildren<Transform>()
                .First(x => childName.Equals(x.name))
                .gameObject;
        }

        private void GeneratePlants(GameObject parentPlants)
        {
            const int numberOfPlants = 100;
            GenerateObjects(parentPlants, numberOfPlants, GeneratePlant);
        }

        private void GenerateRocks(GameObject parentRocks)
        {
            const int numberOfRocks = 10;
            GenerateObjects(parentRocks, numberOfRocks, GenerateRock);
        }

        private void GenerateTree(GameObject parentTrees)
        {
            var treeGo = GenerateObject(parentTrees, _staticData.trees);
            CheckForCollision(treeGo);

            var treeEntity = _world.NewEntity();
            treeEntity.Get<EnvironmentTag>();
            treeEntity.Get<TreeTag>();
            treeEntity.Get<InteractableTag>();
            treeEntity.Get<InteractableComponent>() = new InteractableComponent()
            {
                collider = treeGo.GetComponent<Collider>(),
                transform = treeGo.transform,
                type = InteractableType.Tree
            };
        }

        private void GeneratePlant(GameObject parentPlants)
        {
            GenerateObject(parentPlants, _staticData.plants);
            var plantEntity = _world.NewEntity();
            plantEntity.Get<EnvironmentTag>();
            plantEntity.Get<PlantTag>();
        }

        private void GenerateRock(GameObject parentRocks)
        {
            var rockGo = GenerateObject(parentRocks, _staticData.rocks);
            CheckForCollision(rockGo);

            var rockEntity = _world.NewEntity();
            rockEntity.Get<EnvironmentTag>();
            rockEntity.Get<RockTag>();
            rockEntity.Get<InteractableTag>();
            rockEntity.Get<InteractableComponent>() = new InteractableComponent()
            {
                collider = rockGo.GetComponent<Collider>(),
                transform = rockGo.transform,
                type = InteractableType.Rock
            };
        }

        private void GenerateTrees(GameObject parentTrees)
        {
            const int numberOfTrees = 15;
            GenerateObjects(parentTrees, numberOfTrees, GenerateTree);
        }

        private static void GenerateObjects(GameObject parent, int numberOfObjects, Action<GameObject> generator)
        {
            for (var i = 0; i < numberOfObjects; i++)
            {
                generator.Invoke(parent);
            }
        }

        private GameObject GenerateObject(GameObject parentTrees, GameObject[] prefabsArray)
        {
            var randomPrefab = Randomizer.GetRandomArrayElement(prefabsArray);
            var position = new Vector3
            {
                x = Randomizer.GetRandomInRange(-50, 50),
                z = Randomizer.GetRandomInRange(-50, 50),
            };

            var rotationVector = new Vector3
            {
                y = Randomizer.GetRandomInRange(Constants.MinAngle, Constants.MaxAngle)
            };
            var rotation = Quaternion.Euler(rotationVector);

            return Object.Instantiate(randomPrefab, position, rotation, parentTrees.transform);
        }


        private void CheckForCollision(GameObject gameObject)
        {
            var attempts = 0;

            while (attempts++ < 50)
            {
                var interactableFilter =
                    (EcsFilter<InteractableTag, InteractableComponent>)_world.GetFilter(
                        typeof(EcsFilter<InteractableTag, InteractableComponent>));

                var hasCollision = false;
                foreach (var i in interactableFilter)
                {
                    ref var interactableComponent = ref interactableFilter.Get2(i);
                    // checks for collision
                    if (!interactableComponent.collider.bounds.Intersects(gameObject.GetComponent<Collider>().bounds))
                        continue;
                    // Debug.Log("Collision found");
                    hasCollision = true;
                    break;
                }

                if (!hasCollision)
                {
                    // Debug.Log("fixed collision");
                    return;
                }

                gameObject.transform.position = new Vector3()
                {
                    x = Randomizer.GetRandomInRange(-50, 50),
                    z = Randomizer.GetRandomInRange(-50, 50),
                };
            }

            Debug.Log("couldn't fix collision");
        }
    }
}