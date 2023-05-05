using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    //Rigidbody component reference
    [SerializeField] private Rigidbody _rigidbody;
    
    //Prevent the bullet from never deactivating if nothing is hit
    [SerializeField] private float _lifeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(Vector3 position, Vector3 velocity)
    {
        //Set position and velocity
        transform.position = position;
        _rigidbody.velocity = velocity;
        
        //Activate the GameObject
        gameObject.SetActive(true);
        
        //Start decay coroutine 
        StartCoroutine("Decay");
    }

    private IEnumerator Decay()
    {
        //Decay after lifetime seconds
        yield return new WaitForSeconds(_lifeTime);

        Deactivate();
    }

    public void Deactivate()
    {
        //Put the bullet back into the pool
        BulletPool.main.AddToPool(this);
        
        //Stop all coroutines to prevent errors
        StopAllCoroutines();
        
        //Deactivate the GameObject
        gameObject.SetActive(false);
    }
    
    //OnCollisionEnter can also be used
    private void OnTriggerEnter(Collider other)
    {
        //TODO Add code to handle bullet hits
        Debug.Log("A bullet hit something");
        
        
        //After hitting anything just deactivate the bullet
        Deactivate();
    }
}
