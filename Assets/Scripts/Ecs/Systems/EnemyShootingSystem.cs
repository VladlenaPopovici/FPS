using Ecs.Components;
using Ecs.Data;
using Ecs.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public class EnemyShootingSystem : IEcsRunSystem
    {
        private EcsFilter<EnemyTag, WeaponComponent, ButtonHoldComponent, InteractableComponent> _enemyWeaponFilter;
        private StaticData _staticData;

        private EcsWorld _world;

        public void Run()
        {
            foreach (var i in _enemyWeaponFilter)
            {
                ref var buttonHold = ref _enemyWeaponFilter.Get3(i);

                if (buttonHold.IsButtonHeld)
                {
                    buttonHold.HoldTimer += Time.deltaTime;

                    ref var weapon = ref _enemyWeaponFilter.Get2(i);

                    if (!HasFireRateUpdated(buttonHold, weapon)) continue;

                    var transform = weapon.WeaponTransform;

                    // ref var interactableComponent = ref _enemyWeaponFilter.Get4(i);
                    var muzzleTransform = transform
                        .Find("muzzle")
                        .transform;

                    CreateBullet(muzzleTransform);

                    weapon.LastFireTimestamp = buttonHold.HoldTimer;
                }
                else if (buttonHold.IsButtonReleased)
                {
                    // Reset the hold timer and release state
                    buttonHold.HoldTimer = 0f;
                    buttonHold.IsButtonReleased = false;

                    ref var weapon = ref _enemyWeaponFilter.Get2(i);
                    weapon.LastFireTimestamp = 0;
                }
            }
        }

        private static bool HasFireRateUpdated(ButtonHoldComponent buttonHold, WeaponComponent weapon)
        {
            return buttonHold.HoldTimer - weapon.LastFireTimestamp > weapon.FireRate;
        }

        private void CreateBullet(Transform transform)
        {
            var position = transform.position;
            var rotation = transform.rotation;

            var bulletGo = Object.Instantiate(_staticData.bulletPrefab, position, rotation,
                _staticData.bulletParent.transform);

            var rigidbody = bulletGo.GetComponent<Rigidbody>();

            var bulletEntity = _world.NewEntity();
            bulletEntity.Get<BulletComponent>() = new BulletComponent
            {
                GameObject = bulletGo
            };

            rigidbody.velocity = rigidbody.transform.forward * 5f; // 5 - bullet speed
        }
    }
}