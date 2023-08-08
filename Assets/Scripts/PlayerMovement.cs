using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1, 2)] int _playerNumber;
    [SerializeField] float _speed = 5;
    [SerializeField] float _jumpForce = 200;
    [SerializeField] int _maxJumps=2;


    Rigidbody2D _playerRB;
    PlayerInputActions _playerInputActions;
    Animator _playerAnimator;
    SpriteRenderer _playerSpriteRenderer;
    int _jumpsRemaining;
    

    void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerInputActions = new PlayerInputActions();
        _playerAnimator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        
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

    private void Start()
    {
        _jumpsRemaining = _maxJumps;
    }

    void Update()
    {
        var _horizontalInput = _playerInputActions.Player1.Movement.ReadValue<Vector2>().x * _speed;

        
        _playerRB.velocity = new Vector2(_horizontalInput, _playerRB.velocity.y);

        bool walking = _horizontalInput != 0;

        _playerAnimator.SetBool("Walking", walking);

        if (_horizontalInput!=0)
        {
            _playerSpriteRenderer.flipX = _horizontalInput < 0;
        }
       

    }

   

    void Jump(InputAction.CallbackContext context)
    {
        if (_jumpsRemaining > 0)
        {
            _playerRB.AddForce(Vector2.up * _jumpForce);
            _jumpsRemaining--;

        }

    }

    void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("fire");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        _jumpsRemaining = _maxJumps;  
    }

    void Walk()
    {
        
    }

    void ReadHorizontalInput()
    {
        
    }
}
