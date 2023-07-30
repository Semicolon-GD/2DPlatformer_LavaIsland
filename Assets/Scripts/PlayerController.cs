using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _smoothnessMultiplier;
    [SerializeField][Range(1,2)] int _playerNumber;

    Rigidbody2D _playerRB;
    PlayerInput _playerInput;
    PlayerInputActions _playerInputActions;
    Vector2 _movementDirection;

    void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
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
        _movementDirection = _playerInputActions.Player1.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        var newHorizontal = Mathf.Lerp(
             _playerRB.velocity.x,
             _movementDirection.x * _speed,
             Time.deltaTime * _smoothnessMultiplier);
        _playerRB.velocity = new Vector2(newHorizontal, _playerRB.velocity.y);
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
