using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class ShootingSystem : IEcsRunSystem
    {
        private EcsFilter<ButtonHoldComponent> _buttonHoldFilter;
        private EcsFilter<PlayerTag, WeaponComponent> _playerWeaponFilter;
        private EcsFilter<PlayerTag, ModelComponent, LookDirectionComponent> _playerFilter;

        private EcsWorld _world;
        private StaticData _staticData;
        
        public void Run()
        {
            foreach (var i in _buttonHoldFilter)
            {
                ref var buttonHold = ref _buttonHoldFilter.Get1(i);

                if (buttonHold.isButtonHeld)
                {
                    buttonHold.holdTimer += Time.deltaTime;

                    foreach (var j in _playerWeaponFilter)
                    {
                        ref var weapon = ref _playerWeaponFilter.Get2(j);

                        if (weapon.isFullAuto)
                        {
                            if (!HasFireRateUpdated(buttonHold, weapon)) continue;
                            
                            CreateBullet();
                            weapon.lastFireTimestamp = buttonHold.holdTimer;
                        }
                        else
                        {
                            if (HasWeaponShot(weapon)) continue;
                            
                            CreateBullet();
                            weapon.lastFireTimestamp = buttonHold.holdTimer;
                        }
                    }
                }
                else if (buttonHold.isButtonReleased)
                {
                    // Reset the hold timer and release state
                    buttonHold.holdTimer = 0f;
                    buttonHold.isButtonReleased = false;

                    foreach (var j in _playerWeaponFilter)
                    {
                        ref var weapon = ref _playerWeaponFilter.Get2(j);
                        weapon.lastFireTimestamp = 0;
                    }
                }
            }
        }

        private void CreateBullet()
        {
            Debug.Log("fired");

            foreach (var i in _playerFilter)
            {
                ref var modelComponent = ref _playerFilter.Get2(i);
                var transform = modelComponent.modelTransform.GetChild(0).GetChild(0).GetChild(0).GetChild(0)
                    .Find("muzzle")
                    .transform;

                var bulletGo = Object.Instantiate(_staticData.bulletPrefab, transform.position, transform.rotation);

                var rigidbody = bulletGo.GetComponent<Rigidbody>();
                
                var bulletEntity = _world.NewEntity();
                bulletEntity.Get<BulletComponent>() = new BulletComponent()
                {
                    gameObject = bulletGo
                };
                
                rigidbody.velocity = transform.forward * 5; // 5 - bullet speed
            }
        }

        private static bool HasFireRateUpdated(ButtonHoldComponent buttonHold, WeaponComponent weapon)
        {
            return (buttonHold.holdTimer - weapon.lastFireTimestamp > weapon.fireRate);
        }

        private static bool HasWeaponShot(WeaponComponent weapon)
        {
            return weapon.lastFireTimestamp != 0;
        }
    }
}