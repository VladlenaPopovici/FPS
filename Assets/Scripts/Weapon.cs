using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //References
    private BulletPool _bulletPool;

    [Header("References")] [SerializeField]
    private Transform _fpCamera;

    [SerializeField] private Transform _firePoint;

    [Header("Gun Settings")] [SerializeField]
    private float _firePower;

    [Header("State")] [SerializeField] private bool _IsShooting;
    [SerializeField] private float _fireDelay;
    [SerializeField] private float _fireTimer;


    // Start is called before the first frame update
    void Start()
    {
        _bulletPool = BulletPool.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is trying to shoot
        if (_IsShooting)
        {
            //If the player has recently shot a bullet -> cooldown
            if (_fireTimer > 0)
            {
                _fireTimer -= Time.deltaTime;
            }
            //When the cooldown is over shoot again
            else
            {
               //Reset cooldown timer 
               _fireTimer = _fireDelay;
               
               Shoot();
            }
        }
    }

    public void Shoot()
    {
        //Calculate bullet velocity
        Vector3 bulletVelocity = _fpCamera.forward * _firePower;
        
        //Pick (spawn) bullet from the pool
        _bulletPool.PickFromPool(_firePoint.position, bulletVelocity);
    }

    public void PullTrigger()
    {
        //"Full auto"
        if (_fireDelay > 0)
        {
            _IsShooting = true;
        }
        //"Semi auto"
        else
        {
            Shoot();
        }
    }

    public void ReleaseTrigger()
    {
        //Stop shooting
        _IsShooting = false;
        
        //Set cooldown timer to zero to immediately shoot on next press
        _fireTimer = 0;
    }
}
