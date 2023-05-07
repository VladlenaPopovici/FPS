using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Aiming : MonoBehaviour
{
    [Header("Animator")] [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorParam = "aiming";

    [Header("Camera")] [SerializeField] private Camera _camera;
    [SerializeField] private float _defaultFOV, _aimingFOV;

    [Header("General")] [SerializeField] private float _aimSpeed;

    [Header("Scope")] [SerializeField] private bool _enableScope;
    [SerializeField] private MeshRenderer _weaponRenderer;
    [SerializeField] private GameObject _scopeOverlay;

    private bool _isCLicked = true;

    // Start is called before the first frame update
    void Start()
    {
        if (!_weaponRenderer || !_scopeOverlay)
        {
            _enableScope = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Aim(bool isAiming)
    {
        float blendValue = 0, timeElapsed = 0;
        
        //Show weapon model and hide scope UI
        if (_enableScope)
        {
            _weaponRenderer.enabled = true;
            _scopeOverlay.SetActive(false);
        }

        while (timeElapsed < _aimSpeed)
        {
            //Calculate the transition's progress
            blendValue = timeElapsed / _aimSpeed;
            
            //Blend between animations and calculate camera POV's
            if (isAiming)
            {
                _animator.SetFloat(_animatorParam, blendValue);
                _camera.fieldOfView = Mathf.Lerp(_aimingFOV, _defaultFOV, 1 - blendValue);
            }
            else
            {
                _animator.SetFloat(_animatorParam, 1 - blendValue);
                _camera.fieldOfView = Mathf.Lerp(_aimingFOV, _defaultFOV, blendValue);
            }
            
            //Increase timer
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        //If scope is enabled, hide weapon and show scope UI
        if (_enableScope)
        {
            _weaponRenderer.enabled = !isAiming;
            _scopeOverlay.SetActive(isAiming);
        }
        
        //Confirm/Finalize changes
        if (isAiming)
        {
            _animator.SetFloat(_animatorParam, 1);
            _camera.fieldOfView = _aimingFOV;
        }
        else
        {
            _animator.SetFloat(_animatorParam, 0);
            _camera.fieldOfView = _defaultFOV;
        }
    }

    //Called by the event trigger
    public void OnAim(bool state)
    {
        StopAllCoroutines();
        if (_isCLicked)
        {
            StartCoroutine(Aim(state));
            _isCLicked = false;
        }
        else
        {
            _isCLicked = true;
            StopAllCoroutines();
            state = false;
            StartCoroutine(Aim(state));
        }
    }
}
