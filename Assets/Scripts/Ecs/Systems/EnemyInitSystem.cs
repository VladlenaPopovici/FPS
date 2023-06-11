using System.Linq;
using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class EnemyInitSystem : IEcsInitSystem
    {
        private EcsFilter<AnimatorComponent> _enemyAnimator;

        private EcsWorld _world;
        private StaticData _staticData;

        public void Init()
        {
            GenerateStaticShootingEnemy(new Vector3(5, 0, 5));
            GenerateStaticShootingEnemy(new Vector3(10, 0, 15));
            GenerateChasingEnemy();
        }

        private void GenerateChasingEnemy()
        { 
            var enemyGo = Object.Instantiate(_staticData.enemyChasingPrefab, new Vector3(15, 0, 20),
                new Quaternion(0,180, 0, 1));
            _staticData.chasingEnemy = enemyGo;
            var enemyEntity = _world.NewEntity();
            enemyEntity.Get<EnemyTag>();
            enemyEntity.Get<AnimatorComponent>() = new AnimatorComponent()
            {
                animator = enemyGo.GetComponent<Animator>()
            };
        }

        private void GenerateStaticShootingEnemy(Vector3 position)
        {
            //TODO use static data for position
            var enemyGo = Object.Instantiate(_staticData.enemyStaticPrefab, position, Quaternion.identity);

            var enemyEntity = _world.NewEntity();
            enemyEntity.Get<EnemyTag>();
            var weaponTransform = enemyGo
                .GetComponentsInChildren<Transform>()
                .First(x => "mock_gun".Equals(x.name));
            enemyEntity.Get<WeaponComponent>() = new WeaponComponent()
            {
                isFullAuto = true,
                fireRate = 1f,
                weaponTransform = weaponTransform
            };
            enemyEntity.Get<ButtonHoldComponent>() = new ButtonHoldComponent()
            {
                isButtonHeld = true // hack this enemy is always shooting
            };
            enemyEntity.Get<InteractableTag>();
            enemyEntity.Get<InteractableComponent>() = new InteractableComponent()
            {
                collider = enemyGo.GetComponent<Collider>(),
                transform = enemyGo.transform,
                type = InteractableType.Enemy,
            };
        }
    }
}