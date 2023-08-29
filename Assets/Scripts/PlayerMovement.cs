using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1, 2)] int _playerNumber;
    [SerializeField] Transform _feet;
    [SerializeField] float _speed = 5;
    [SerializeField] float _jumpVelocity = 10;
    [SerializeField] int _maxJumps=2;
    [SerializeField] float _downPull=5;

    Rigidbody2D _playerRB;
    PlayerInputActions _playerInputActions;
    Animator _playerAnimator;
    SpriteRenderer _playerSpriteRenderer;
    int _jumpsRemaining;
    bool _isGrounded;
    float _fallTimer;
    string _player;


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

    void Start()
    {
        _jumpsRemaining = _maxJumps;
    }

    void Update()
    {
        var _horizontalInput = _playerInputActions.Player1.Movement.ReadValue<Vector2>().x * _speed;
           //  _horizontalInput = _playerInputActions.Player2.Movement.ReadValue<Vector2>().x * _speed;


        _playerRB.velocity = new Vector2(_horizontalInput, _playerRB.velocity.y);

        bool walking = _horizontalInput != 0;

        _playerAnimator.SetBool("Walking", walking);

        if (_horizontalInput!=0)
        {
            _playerSpriteRenderer.flipX = _horizontalInput < 0;
        }

        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        _isGrounded = (hit != null) && (_playerRB.velocity.y<=0);

        if (_isGrounded)
        {
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;
          //  Debug.Log("grounded");
        }
        else
        {
            _fallTimer += Time.deltaTime;
            var downForce = _downPull * _fallTimer * _fallTimer;
            _playerRB.velocity = new Vector2(_playerRB.velocity.x,_playerRB.velocity.y-downForce);
        }

    }

  


    void Jump(InputAction.CallbackContext context)
    {
        if (_jumpsRemaining > 0 && context.interaction is PressInteraction)
        {
            _playerRB.velocity=new Vector2(_playerRB.velocity.x,_jumpVelocity);
            _jumpsRemaining--;
            _fallTimer = 0;

        }
        else if (_jumpsRemaining > 0 && context.interaction is HoldInteraction)
        {
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, _jumpVelocity*1.5f);
            _jumpsRemaining--;
            _fallTimer = 0;

        }
       // Debug.Log(context.interaction);
    }

    void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("fire");
    }


    void Walk()
    {
        
    }

    void ReadHorizontalInput()
    {
        
    }
}
