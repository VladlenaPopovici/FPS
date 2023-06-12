using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class PlayerShootingSystem : IEcsRunSystem
    {
        private EcsFilter<ShootingButtonTag, ButtonHoldComponent> _buttonHoldFilter;
        private EcsFilter<PlayerTag, ModelComponent, LookDirectionComponent> _playerFilter;
        private EcsFilter<PlayerTag, WeaponComponent> _playerWeaponFilter;

        private StaticData _staticData;
        private EcsWorld _world;

        public void Run()
        {
            foreach (var i in _buttonHoldFilter)
            {
                ref var buttonHold = ref _buttonHoldFilter.Get2(i);

                if (buttonHold.IsButtonHeld)
                {
                    buttonHold.HoldTimer += Time.deltaTime;

                    foreach (var j in _playerWeaponFilter)
                    {
                        ref var weapon = ref _playerWeaponFilter.Get2(j);

                        if (weapon.IsFullAuto)
                        {
                            if (!HasFireRateUpdated(buttonHold, weapon)) continue;

                            CreateBullet();
                            weapon.LastFireTimestamp = buttonHold.HoldTimer;
                        }
                        else
                        {
                            if (HasWeaponShot(weapon)) continue;

                            CreateBullet();
                            weapon.LastFireTimestamp = buttonHold.HoldTimer;
                        }
                    }
                }
                else if (buttonHold.IsButtonReleased)
                {
                    // Reset the hold timer and release state
                    buttonHold.HoldTimer = 0f;
                    buttonHold.IsButtonReleased = false;

                    foreach (var j in _playerWeaponFilter)
                    {
                        ref var weapon = ref _playerWeaponFilter.Get2(j);
                        weapon.LastFireTimestamp = 0;
                    }
                }
            }
        }

        private void CreateBullet()
        {
            foreach (var i in _playerFilter)
            {
                ref var modelComponent = ref _playerFilter.Get2(i);
                var transform = modelComponent.modelTransform.GetChild(0).GetChild(0).GetChild(0).GetChild(0)
                    .Find("muzzle")
                    .transform;

                var bulletGo = Object.Instantiate(_staticData.bulletPrefab, transform.position, transform.rotation,
                    _staticData.bulletParent.transform);

                var rigidbody = bulletGo.GetComponent<Rigidbody>();

                var bulletEntity = _world.NewEntity();
                bulletEntity.Get<BulletComponent>() = new BulletComponent
                {
                    GameObject = bulletGo
                };

                rigidbody.velocity = transform.forward * 5; // 5 - bullet speed
            }
        }

        private static bool HasFireRateUpdated(ButtonHoldComponent buttonHold, WeaponComponent weapon)
        {
            return buttonHold.HoldTimer - weapon.LastFireTimestamp > weapon.FireRate;
        }

        private static bool HasWeaponShot(WeaponComponent weapon)
        {
            return weapon.LastFireTimestamp != 0;
        }
    }
}