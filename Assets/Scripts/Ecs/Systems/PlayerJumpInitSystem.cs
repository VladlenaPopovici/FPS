using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ecs.Systems
{
    public sealed class PlayerJumpInitSystem : IEcsInitSystem
    {
        private EcsFilter<PlayerComponent, JumpComponent> _jumpFilter;
        private EcsFilter<ModelComponent, MovableComponent, DirectionComponent> movableFilter;

        private EcsWorld _world;
        private StaticData _staticData;

        private Button _jumpButton;

        public void Init()
        {
            _jumpButton = Object.Instantiate(_staticData.jumpButtonPrefab, Constants.buttonsPanel);
            var jumpEntity = _world.NewEntity();
            jumpEntity.Get<JumpTag>();
            jumpEntity.Get<ButtonComponent>() = new ButtonComponent()
            {
                button = _jumpButton,
                isVisible = true
            };

            _jumpButton.onClick.AddListener(Jump);
        }

        private void Jump()
        {
            foreach (var j in _jumpFilter)
            {
                ref var jumpComponent = ref _jumpFilter.Get2(j);
                foreach (var i in movableFilter)
                {
                    ref var movableComponent = ref movableFilter.Get2(i);
                    ref var characterController = ref movableComponent.characterController;

                    jumpComponent.playerVelocity = characterController.transform.position;

                    if (!characterController.isGrounded) return;

                    jumpComponent.playerVelocity.y +=
                        Mathf.Sqrt(jumpComponent.jumpForce * -3.0f * jumpComponent.gravity);

                    characterController.Move(jumpComponent.playerVelocity * 10 * Time.deltaTime);
                }
            }
        }
    }
}