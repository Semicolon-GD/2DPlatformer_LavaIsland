using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField][Range(1, 2)] int _playerNumber;
    [SerializeField] Transform _feet;
    [SerializeField] float _speed;
    [SerializeField] float _slipFactor;
    [SerializeField] float _acceleration;
    [SerializeField] float _breaking;
    [SerializeField] float _airAcceleration;
    [SerializeField] float _airBreaking;

    Rigidbody2D _playerRB;
    PlayerInput _playerInput;
    PlayerInputActions _playerInputActions;
    Vector2 _movementVector;
    float _horizontal;
    int _layerMask;
    bool _isGrounded;
    bool _isOnSlipperySurface;
   

    void Awake()
    {
        
        _playerRB = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _layerMask = LayerMask.GetMask("Default", "Mushroom");
        switch (_playerNumber)
        {
            case 1:
                _playerInputActions.Player2.Disable();
                _playerInputActions.Player1.Enable();
                _playerInputActions.Player1.Fire.performed += Fire;
                _playerInputActions.Player1.Jump.performed += Jump;
                break;
            case 2:
                _playerInputActions.Player1.Disable();
                _playerInputActions.Player2.Enable();
                _playerInputActions.Player2.Fire.performed += Fire;
                _playerInputActions.Player2.Jump.performed += Jump;
                break;
            default:
                break;
        }

    }

    

    void Update()
    {
        UpdateIsGrounded();
        ReadWalkingInput();
        if (_isOnSlipperySurface)
            SlipperyWalk();
        else
            Walk();

        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        //if (_ho)
        //{

        //}
    }

    private void UpdateIsGrounded()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, .1f, _layerMask);

        _isGrounded = hit != null;

        _isOnSlipperySurface = hit?.CompareTag("Slippery") ?? false;

    }

    private void ReadWalkingInput()
    {
        _horizontal = _playerInputActions.Player1.Movement.ReadValue<float>()*_speed;
       // _horizontal = _playerInputActions.Player1.Movement.ReadValue<Vector2>().x * _speed;
        if (_playerInputActions.Player1.Movement.ReadValue<Vector2>() != new Vector2(0, 0))
            Debug.Log(_playerInputActions.Player1.Movement.ReadValue<Vector2>() + " hor " + _horizontal);
    }

    private void FixedUpdate()
    {
        
        
    }

    private void Walk()
    {
        float smoothnessMultiplier = _horizontal == 0 ? _breaking : _acceleration;
        if (_isGrounded == false)
            smoothnessMultiplier = _horizontal == 0 ? _airBreaking : _airAcceleration;

        var newHorizontal = Mathf.Lerp(
                     _playerRB.velocity.x,
                     _horizontal * _speed,
                     Time.deltaTime * smoothnessMultiplier);
        _playerRB.velocity = new Vector2(newHorizontal, _playerRB.velocity.y);
    }

    

    private void SlipperyWalk()
    {
        var desiredVelocity = new Vector2(_horizontal * _speed, _playerRB.velocity.y);
        var smoothedVelocity = Vector2.Lerp(
            _playerRB.velocity,
            desiredVelocity,
            Time.deltaTime / _slipFactor);
        _playerRB.velocity = smoothedVelocity;
       // Debug.Log("horizontal" + _horizontal + " desired " + desiredVelocity + " smooth " + smoothedVelocity + " playervelo "+_playerRB.velocity+" time "+Time.deltaTime + " time2 "+Time.deltaTime/_slipFactor);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump "+context.phase);
    }

    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire "+ context.phase);
    }
}
