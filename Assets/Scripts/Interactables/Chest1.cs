using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest1 : Interactable
{
    [Header("Settings")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animParam = "open";
    [SerializeField] private Button _button;
    
    private bool _isClicked = true;
    
    //TODO Should be count in script where will be logic of inventory
    private float _waitTime = 3;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //There will be designed interaction using code
    public override void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        StartCoroutine(OpenChest());
    }
    
    private IEnumerator OpenChest()
    {
        _animator.SetFloat(_animParam, 1);
        yield return new WaitForSeconds(_waitTime);
        _animator.SetFloat(_animParam, 0);
    }    
}
