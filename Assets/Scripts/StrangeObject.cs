using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeObject : MonoBehaviour
{
    [SerializeField] private float _damage = 10;

    // Add a collider to this enemy obstacle
    private void Awake()
    {
        SphereCollider sc = gameObject.AddComponent<SphereCollider>();
        sc.isTrigger = true; // make it a trigger to use OnTriggerEnter
    }

    // The OnTriggerEnter method gets called when a collision happens
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<FirstPersonController>();
        if (player != null)
        {
            player.TakeDamage(_damage);
        }

    }
}
