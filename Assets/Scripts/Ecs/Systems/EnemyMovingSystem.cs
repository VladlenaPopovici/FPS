using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class EnemyMovingSystem : IEcsRunSystem
    {
        private EcsFilter<MovableComponent> _playerFilter;
        private EcsFilter<EnemyTag, ChasingEnemyTag, AnimatorComponent, ButtonHoldComponent, InteractableComponent> _enemyFilter;

        private StaticData _staticData;

        public void Run()
        {
            foreach (var i in _playerFilter)
            {
                ref var playerComponent = ref _playerFilter.Get1(i);
                ref var playerCharacterController = ref playerComponent.characterController;

                var position = playerCharacterController.transform.position;
                position.y -= 1;
                
                foreach (var j in _enemyFilter)
                {
                    ref var interactableComponent = ref _enemyFilter.Get5(j);
                    var enemy = interactableComponent.transform.gameObject;
                    
                    var distance = Vector3.Distance(enemy.transform.position, position);
                    var animatorComponent = _enemyFilter.Get3(j);
                    animatorComponent.animator.SetBool("Idle", false);
                    animatorComponent.animator.SetBool("IsWalking", false);
                    animatorComponent.animator.SetBool("IsShooting", false);
                    if (distance < 5)
                    {
                        animatorComponent.animator.SetBool("IsShooting", true);
                        
                        ref var buttonHoldComponent = ref _enemyFilter.Get4(i);
                        buttonHoldComponent.isButtonHeld = true;
                    } 
                    else if (distance < 15)
                    {
                        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position,
                            position, Time.deltaTime);
                        enemy.transform.LookAt(position);
                        animatorComponent.animator.SetBool("IsWalking", true);
                        
                        ref var buttonHoldComponent = ref _enemyFilter.Get4(i);
                        buttonHoldComponent.isButtonHeld = false;
                    }
                    else
                    {
                        animatorComponent.animator.SetBool("Idle", true);
                    }
                }
            }
        }
    }
}