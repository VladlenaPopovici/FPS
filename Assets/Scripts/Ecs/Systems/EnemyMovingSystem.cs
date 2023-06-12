using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class EnemyMovingSystem : IEcsRunSystem
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        private EcsFilter<EnemyTag, ChasingEnemyTag, AnimatorComponent, ButtonHoldComponent, InteractableComponent>
            _enemyFilter;

        private EcsFilter<MovableComponent> _playerFilter;

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
                    animatorComponent.Animator.SetBool(Idle, false);
                    animatorComponent.Animator.SetBool(IsWalking, false);
                    animatorComponent.Animator.SetBool(IsShooting, false);
                    if (distance < 5)
                    {
                        animatorComponent.Animator.SetBool(IsShooting, true);

                        ref var buttonHoldComponent = ref _enemyFilter.Get4(i);
                        buttonHoldComponent.IsButtonHeld = true;
                    }
                    else if (distance < 15)
                    {
                        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position,
                            position, Time.deltaTime);
                        enemy.transform.LookAt(position);
                        animatorComponent.Animator.SetBool(IsWalking, true);

                        ref var buttonHoldComponent = ref _enemyFilter.Get4(i);
                        buttonHoldComponent.IsButtonHeld = false;
                    }
                    else
                    {
                        animatorComponent.Animator.SetBool(Idle, true);
                    }
                }
            }
        }
    }
}