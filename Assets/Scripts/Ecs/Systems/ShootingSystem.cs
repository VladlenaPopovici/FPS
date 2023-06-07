using Leopotam.Ecs;
using UnityEngine;

namespace Ecs.Systems
{
    public sealed class ShootingSystem : IEcsRunSystem
    {
        private EcsFilter<ButtonHoldComponent> _buttonHoldFilter;
        private EcsFilter<PlayerTag, WeaponComponent> _playerWeaponFilter;
            
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
                            if (!(buttonHold.holdTimer - weapon.lastFireTimestamp > weapon.fireRate)) continue;
                            
                            Debug.Log("fired");
                            weapon.lastFireTimestamp = buttonHold.holdTimer;
                        }
                        else
                        {
                            if (weapon.lastFireTimestamp != 0) continue;
                            
                            Debug.Log("fired");
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
    }
}