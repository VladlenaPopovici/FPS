using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;
using Utils;

namespace Ecs
{
    public sealed class ChestInitSystem : IEcsInitSystem
    {
        private StaticData _staticData;
        private const float minX = -50;
        private const float maxX = 50;
        private const float minZ = -50;
        private const float maxZ = 50;

        public void Init()
        {
            var parentChest = Object.Instantiate(_staticData.parentChest);
            
            for (int i = 0; i < 10; i++)
            {
                var position = new Vector3
                {
                    x = Randomizer.GetRandomInRange(minX, maxX),
                    z = Randomizer.GetRandomInRange(minZ, maxZ),
                    y = 0.3f
                };

                var rotationVector = new Vector3
                {
                    y = Randomizer.GetRandomInRange(Constants.MinAngle, Constants.MaxAngle)
                };
                var rotation = Quaternion.Euler(rotationVector);


                var chest = Object.Instantiate(_staticData.chestPrefab, position, rotation, parentChest.transform);

            }
        }
    }
}