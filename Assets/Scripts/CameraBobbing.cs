using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("Transform references")] [SerializeField]
    private Transform _headTransform;
    [SerializeField] private Transform _cameraTransform;

    [Header("Head bobbing")] [SerializeField]
    private float _bobFrequency = 5f;
    [SerializeField] private float _bobHorizontalAmplitude = 0.1f;
    [SerializeField] private float _bobVerticalAmplitude = 0.1f;
    [Range(0, 1)] [SerializeField] private float _headSmoothing = 0.1f;
    
    //State
    public bool isWalking;
    private float _walkingTime;
    private Vector3 _targetCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Set time and offset to 0
        if (!isWalking)
        {
            _walkingTime = 0;
        }
        else
        {
            _walkingTime += Time.deltaTime;
        }
        
        //Calculate the camera's target position
        _targetCameraPosition = _headTransform.position + CalculateHeadBobOffset(_walkingTime);
        
        //Interpolate position
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _targetCameraPosition, _headSmoothing);
        
        //Snap to position if it is close enough
        if ((_cameraTransform.position - _targetCameraPosition).magnitude <= 0.001)
        {
            _cameraTransform.position = _targetCameraPosition;
        }
    }

    private Vector3 CalculateHeadBobOffset(float t)
    {
        float horizontalOffSet = 0;
        float verticalOffset = 0;
        Vector3 offset = Vector3.zero;

        if (t > 0)
        {
            //Calculate offsets
            horizontalOffSet = Mathf.Cos(t * _bobFrequency) * _bobHorizontalAmplitude;
            verticalOffset = Mathf.Sin(t * _bobFrequency * 2) * _bobVerticalAmplitude;
            
            //Combine offsets relative to the head's position and calculate the camera's target position
            offset = _headTransform.right * horizontalOffSet + _headTransform.up * verticalOffset;
        }

        return offset;
    }
}
