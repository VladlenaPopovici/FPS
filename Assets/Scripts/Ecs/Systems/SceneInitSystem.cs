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
            const int numberOfPlants = 50;
            GenerateObjects(parentPlants, numberOfPlants, GeneratePlant);
        }

        private void GenerateRocks(GameObject parentRocks)
        {
            const int numberOfRocks = 10;
            GenerateObjects(parentRocks, numberOfRocks, GenerateRock);
        }

        private GameObject GenerateTree(GameObject parentTrees)
        {
            return GenerateObject(parentTrees, _staticData.trees);
        }
        
        private GameObject GeneratePlant(GameObject parentPlants)
        {
            return GenerateObject(parentPlants, _staticData.plants);
        }
        
        private GameObject GenerateRock(GameObject parentRocks)
        {
            return GenerateObject(parentRocks, _staticData.rocks);
        }

        private void GenerateTrees(GameObject parentTrees)
        {
            const int numberOfTrees = 15;
            GenerateObjects(parentTrees, numberOfTrees, GenerateTree);
            
        }

        private void GenerateObjects(GameObject parent, int numberOfObjects, Func<GameObject, GameObject> generator)
        {
            for (var i = 0; i < numberOfObjects; i++)
            {
                var tree = generator.Invoke(parent);
            }
        }

        private static GameObject GenerateObject(GameObject parentTrees, GameObject[] prefabsArray)
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
    }
}