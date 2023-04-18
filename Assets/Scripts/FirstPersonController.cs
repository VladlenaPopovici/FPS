using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    //References
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private CharacterController _characterController;
    
    //Player settings
    [SerializeField] private float _cameraSensitivity;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveInputDeadZone;
        
    //Touch detection
    private int _leftFingerId, _rightFingerId;
    private float _halfScreenWidth;
    
    //Camera control
    private Vector2 _lookInput;
    private float _cameraPitch;
    
    //Player movement
    private Vector2 _moveTouchStartPosition;
    private Vector2 _moveInput;
    
    // Start is called before the first frame update
    void Start()
    {
        //ID -1 means that the finger does not touch the screen
        _leftFingerId = -1;
        _rightFingerId = -1;

        //only calculate once
        _halfScreenWidth = Screen.width / 2;
        
        //calculate the movement input dead zone
        _moveInputDeadZone = Mathf.Pow(Screen.height / _moveInputDeadZone, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //Handles input
        GetTouchInput();

        if (_rightFingerId != -1)
        {
            //Only look around if right finger is being tracking
            Debug.Log("Rotating");
            LookAround();
        }

        if (_leftFingerId != -1)
        {
            //Only move if left finger is being tracked
            Debug.Log("Moving");
            Move();
        }
    }

    private void GetTouchInput()
    {
        //Iterate through all detected touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            
            //Check ech touch's phase
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (touch.position.x < _halfScreenWidth && _leftFingerId == -1)
                    {
                        //Start tracking the left finger if it was not tracking before
                        _leftFingerId = touch.fingerId;

                        //Set the start position for the movement control finger
                        _moveTouchStartPosition = touch.position;
                    }
                    else if (touch.position.x > _halfScreenWidth && _rightFingerId == -1)
                    {
                        //Start tracking the right finger if it was not tracking before
                        _rightFingerId = touch.fingerId;
                        Debug.Log("Tracking right finger");
                    }
                    
                    break;
                
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (touch.fingerId == _leftFingerId)
                    {
                        //Stop tracking the left finger
                        _leftFingerId = -1;
                        Debug.Log("Stop tracking left finger");
                    } else if (touch.fingerId == _rightFingerId)
                    {
                        //Stop tracking the right finger   
                        _rightFingerId = -1;
                        Debug.Log("Stop tracking right finger");
                    }

                    break;
                    
                case TouchPhase.Moved:
                    //Get input for looking around
                    if (touch.fingerId == _rightFingerId)
                    {
                        _lookInput = touch.deltaPosition * (_cameraSensitivity * Time.deltaTime);
                    } else if (touch.fingerId == _leftFingerId) 
                    {
                        //Calculating the position delta from the start  position
                        _moveInput = touch.position - _moveTouchStartPosition;
                    }

                    break;
                case TouchPhase.Stationary:
                    //Set the look input to zero if the finger is still
                    if (touch.fingerId == _rightFingerId)
                    {
                        _lookInput = Vector2.zero;
                    }

                    break;
            }
        }
    }

    private void LookAround()
    {
        //Vertical (pitch) rotation
        _cameraPitch = Mathf.Clamp(_cameraPitch - _lookInput.y, -90f, 90);
        _cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0, 0);
        
        //Horizontal (yaw) rotation
        transform.Rotate(transform.up, _lookInput.x);
    }

    private void Move()
    {
        //Don't move if the touch delta is shorter than the deigned dead zone
        if (_moveInput.sqrMagnitude <= _moveInputDeadZone) return;
        
        //Multiply the normalized direction by the speed
        Vector2 movementDirection = _moveInput.normalized * _moveSpeed * Time.deltaTime;
        
        //Move relatively to the local transform's direction
        _characterController.Move(transform.right * movementDirection.x + transform.forward * movementDirection.y);
    }
}

    
