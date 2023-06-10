using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public class EnemyShootingSystem : IEcsRunSystem
    {
        private EcsFilter<EnemyTag, WeaponComponent, ButtonHoldComponent> _enemyWeaponFilter;

        private EcsWorld _world;
        private StaticData _staticData;

        public void Run()
        {
            foreach (var i in _enemyWeaponFilter)
            {
                ref var buttonHold = ref _enemyWeaponFilter.Get3(i);

                if (buttonHold.isButtonHeld)
                {
                    buttonHold.holdTimer += Time.deltaTime;

                    ref var weapon = ref _enemyWeaponFilter.Get2(i);

                    if (!HasFireRateUpdated(buttonHold, weapon)) continue;

                    var transform = weapon.weaponTransform;
                    CreateBullet(transform);

                    weapon.lastFireTimestamp = buttonHold.holdTimer;
                }
                else if (buttonHold.isButtonReleased)
                {
                    // Reset the hold timer and release state
                    buttonHold.holdTimer = 0f;
                    buttonHold.isButtonReleased = false;

                    ref var weapon = ref _enemyWeaponFilter.Get2(i);
                    weapon.lastFireTimestamp = 0;
                }
            }
        }

        private static bool HasFireRateUpdated(ButtonHoldComponent buttonHold, WeaponComponent weapon)
        {
            return (buttonHold.holdTimer - weapon.lastFireTimestamp > weapon.fireRate);
        }

        private void CreateBullet(Transform transform)
        {
            var position = transform.position;

            // fixing animation 1
            var newPosition = new Vector3
            {
                x = position.x - 0.47f,
                y = position.y + .45f,
                z = position.z + .6f
            };
            var bulletGo = Object.Instantiate(_staticData.bulletPrefab, newPosition, transform.rotation, _staticData.bulletParent.transform);

            var rigidbody = bulletGo.GetComponent<Rigidbody>();

            var bulletEntity = _world.NewEntity();
            bulletEntity.Get<BulletComponent>() = new BulletComponent()
            {
                gameObject = bulletGo
            };

            // fixing animation 2
            var correctForward = Quaternion.identity * Vector3.forward;
            rigidbody.velocity = correctForward * 5; // 5 - bullet speed
        }
    }
}