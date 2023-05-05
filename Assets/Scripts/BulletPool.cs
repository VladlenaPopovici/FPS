using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //Singleton reference
    public static BulletPool main;
    
    //Settings
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _poolSize = 100;
    
    //Can also use Queue<Bullet>
    private List<Bullet> _availableBullets;

    private void Awake()
    {
        //Initialize singleton
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _availableBullets = new List<Bullet>();

        for (int i = 0; i < _poolSize; i++)
        {
            //Instantiate bullet clone
            Bullet bullet = Instantiate(_bulletPrefab, transform).GetComponent<Bullet>();
            bullet.gameObject.SetActive(false);
            
            //Add it to the pool
            _availableBullets.Add(bullet);
        }
    }

    public void PickFromPool(Vector3 position, Vector3 velocity)
    {
        //Prevent errors
        if (_availableBullets.Count < 1) return;
        
        //Activate the bullet
        _availableBullets[0].Activate(position, velocity);
        
        //Pop up from the list
        _availableBullets.RemoveAt(0);
    }

    public void AddToPool(Bullet bullet)
    {
        //Add the bullet (back) to the pool
        if (!_availableBullets.Contains(bullet))
        {
            _availableBullets.Add(bullet);
        }
    }
}
