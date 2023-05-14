using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest1 : Interactable
{
    [Header("Settings")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animParam = "open";
    [SerializeField] private Image _button;
    [SerializeField] private Canvas _playerUI;
    [SerializeField] private Canvas _chestInventoryUI;
    [SerializeField] private Canvas _playerInventoryUI;
    [SerializeField] private Button _closeInventory;
    
    private bool _isClicked = true;
    
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
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        _playerUI.gameObject.SetActive(false);
        _chestInventoryUI.gameObject.SetActive(true);
        
        yield return null;
    }

    public void CloseAll()
    {        
        _playerUI.gameObject.SetActive(true);
        _chestInventoryUI.gameObject.SetActive(false);
        _animator.SetFloat(_animParam, 0);

        Time.timeScale = 1;
    }
}
