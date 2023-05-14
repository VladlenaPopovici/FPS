using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _distance = 3f;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Image _button;

    private PlayerUI _playerUI;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Message and button get clean when player is not looking at the interactable
        _playerUI.UpdateText(string.Empty);
        _button.gameObject.SetActive(false);
        //Creating a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _distance);
        
        //The variable to store collision information
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, _distance, _mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                _playerUI.UpdateText(hitInfo.collider.GetComponent<Interactable>().promptMessage);
                _button.gameObject.SetActive(true);
            }
        }
    }
}
