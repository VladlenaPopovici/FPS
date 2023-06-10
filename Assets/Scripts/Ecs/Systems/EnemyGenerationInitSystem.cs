using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class EnemyGenerationInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private StaticData _staticData;
        
        public void Init()
        {
            GenerateStaticShootingEnemy();
            GenerateChasingEnemy();
        }

        private void GenerateChasingEnemy()
        {
            
        }

        private void GenerateStaticShootingEnemy()
        {
            //TODO use static data for position
            var position = new Vector3(10, 0, 15);
            var enemyGo = Object.Instantiate(_staticData.enemyPrefab, position, Quaternion.identity);

            var enemyEntity = _world.NewEntity();
            enemyEntity.Get<EnemyTag>();
            enemyEntity.Get<WeaponComponent>() = new WeaponComponent()
            {
                isFullAuto = true,
                fireRate = 1f
            };
            enemyEntity.Get<InteractableTag>();
            enemyEntity.Get<InteractableComponent>() = new InteractableComponent()
            {
                collider = enemyGo.GetComponent<Collider>(),
                transform = enemyGo.transform,
                type = InteractableType.Enemy
            };
        }
    }
}