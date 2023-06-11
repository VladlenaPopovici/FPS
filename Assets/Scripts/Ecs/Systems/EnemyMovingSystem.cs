using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class EnemyMovingSystem : IEcsRunSystem
    {
        private EcsFilter<MovableComponent> _playerFilter;
        private EcsFilter<AnimatorComponent> _enemyFilter;

        private StaticData _staticData;

        public void Run()
        {
            foreach (var i in _playerFilter)
            {
                ref var playerComponent = ref _playerFilter.Get1(i);
                ref var playerCharacterController = ref playerComponent.characterController;

                var enemy = _staticData.chasingEnemy;

                var position = playerCharacterController.transform.position;
                position.y -= 1;

                float distance = Vector3.Distance(enemy.transform.position, position);
                
                foreach (var j in _enemyFilter)
                {
                    var animatorComponent = _enemyFilter.Get1(j);
                    if (distance < 5)
                    {
                        animatorComponent.animator.SetBool("IsShooting", true);
                        animatorComponent.animator.SetBool("IsWalking", false);
                        animatorComponent.animator.SetBool("Idle", false);
                    } 
                    else if (distance < 15)
                    {
                        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position,
                            position, Time.deltaTime);
                        enemy.transform.LookAt(position);
                        animatorComponent.animator.SetBool("IsShooting", false);
                        animatorComponent.animator.SetBool("IsWalking", true);
                        animatorComponent.animator.SetBool("Idle", false);
                    }
                    else
                    {
                        animatorComponent.animator.SetBool("IsShooting", false);
                        animatorComponent.animator.SetBool("IsWalking", false);
                        animatorComponent.animator.SetBool("Idle", true);
                    }
                }
            }
        }
    }
}