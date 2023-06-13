using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ecs.Systems
{
    public sealed class PlayerJumpInitSystem : IEcsInitSystem
    {
        private Button _jumpButton;
        private EcsFilter<PlayerComponent, JumpComponent> _jumpFilter;
        private EcsFilter<ModelComponent, MovableComponent, DirectionComponent> _movableFilter;

        private StaticData _staticData;
        private EcsWorld _world;

        public void Init()
        {
            _jumpButton = Object.Instantiate(_staticData.jumpButtonPrefab, Constants.ButtonsPanel);
            var jumpEntity = _world.NewEntity();
            jumpEntity.Get<JumpTag>();
            jumpEntity.Get<ButtonComponent>() = new ButtonComponent
            {
                Button = _jumpButton
            };

            _jumpButton.onClick.AddListener(Jump);
        }

        private void Jump()
        {
            foreach (var j in _jumpFilter)
            {
                ref var jumpComponent = ref _jumpFilter.Get2(j);
                foreach (var i in _movableFilter)
                {
                    ref var movableComponent = ref _movableFilter.Get2(i);
                    ref var characterController = ref movableComponent.characterController;

                    jumpComponent.PlayerVelocity = characterController.transform.position;

                    if (!characterController.isGrounded) return;

                    jumpComponent.PlayerVelocity.y +=
                        Mathf.Sqrt(jumpComponent.JumpForce * -3.0f * jumpComponent.Gravity);

                    characterController.Move(jumpComponent.PlayerVelocity * 10 * Time.deltaTime);
                }
            }
        }
    }
}