using System.Linq;
using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class EnemyInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private StaticData _staticData;

        public void Init()
        {
            GenerateStaticShootingEnemy(new Vector3(5, 0, 5), Quaternion.identity);
            GenerateStaticShootingEnemy(new Vector3(10, 0, 15), Quaternion.Euler(new Vector3(0, 90)));
            GenerateChasingEnemy();
        }

        private void GenerateChasingEnemy()
        { 
            var enemyGo = Object.Instantiate(_staticData.enemyChasingPrefab, new Vector3(15, 0, 20),
                new Quaternion(0,180, 0, 1));
            var enemyEntity = _world.NewEntity();
            SetEnemyComponents(ref enemyEntity, enemyGo);

            enemyEntity.Get<AnimatorComponent>() = new AnimatorComponent()
            {
                animator = enemyGo.GetComponent<Animator>()
            };
            enemyEntity.Get<ChasingEnemyTag>();
        }

        private void GenerateStaticShootingEnemy(Vector3 position, Quaternion rotation)
        {
            //TODO use static data for position
            var enemyGo = Object.Instantiate(_staticData.enemyPrefab, position, rotation);
            Debug.Log(enemyGo);
            var enemyEntity = _world.NewEntity();
            SetEnemyComponents(ref enemyEntity, enemyGo);
            ref var buttonHoldComponent = ref enemyEntity.Get<ButtonHoldComponent>();
            buttonHoldComponent.isButtonHeld = true; // hack so that enemy is always shooting
        }
        
        private static void SetEnemyComponents(ref EcsEntity enemyEntity, GameObject enemyGo)
        {
            enemyEntity.Get<EnemyTag>();
            var weaponTransform = enemyGo
                .GetComponentsInChildren<Transform>()
                .First(x => "Pistol_00".Equals(x.name));
            enemyEntity.Get<WeaponComponent>() = new WeaponComponent()
            {
                isFullAuto = true,
                fireRate = 1f,
                weaponTransform = weaponTransform
            };
            Debug.Log(weaponTransform, weaponTransform.gameObject);
            enemyEntity.Get<ButtonHoldComponent>() = new ButtonHoldComponent()
            {
                isButtonHeld = false
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