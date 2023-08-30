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
    [SerializeField] Transform _leftSensor;
    [SerializeField] Transform _rightSensor;
    [SerializeField]float _wallSlideSpeed=1f;



    Rigidbody2D _playerRB;
    PlayerInputActions _playerInputActions;
    Animator _playerAnimator;
    SpriteRenderer _playerSpriteRenderer;
    int _jumpsRemaining;
    bool _isGrounded;
    float _fallTimer;
    string _player;
    float _horizontalInput;
    AudioSource _jumpSound;
    

    void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerInputActions = new PlayerInputActions();
        _playerAnimator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _jumpSound = GetComponent<AudioSource>();
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
        UpdateIsGrounded();
        ReadMovementInput();
        Move();
        UpdateAnimator();

        UpdateSpriteDirection();

        if (ShouldSlide())
        {
            Slide();
        }

        JumpTimerControll();

    }

    void Slide()
    {
        _playerRB.velocity = new Vector2(_playerRB.velocity.x, -_wallSlideSpeed);
    }
    private void JumpTimerControll()
    {
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
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, _playerRB.velocity.y - downForce);
        }
    }

    private void UpdateSpriteDirection()
    {
        if (_horizontalInput != 0)
        {
            _playerSpriteRenderer.flipX = _horizontalInput < 0;
        }
    }

    private void Move()
    {
        _playerRB.velocity = new Vector2(_horizontalInput, _playerRB.velocity.y);
    }

    private void UpdateAnimator()
    {
        bool walking = _horizontalInput != 0;

        _playerAnimator.SetBool("Walking", walking);
    }

    private void UpdateIsGrounded()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        _isGrounded = (hit != null) && (_playerRB.velocity.y <= 0);
    }

    private void ReadMovementInput()
    {
        _horizontalInput = _playerInputActions.Player1.Movement.ReadValue<Vector2>().x * _speed;
        //  _horizontalInput = _playerInputActions.Player2.Movement.ReadValue<Vector2>().x * _speed;
    }



    

    bool ShouldSlide()
    {
        if (_isGrounded)
            return false;
        if (_playerRB.velocity.y > 0)
            return false;
        if (_horizontalInput < 0)
        {
            var hit = Physics2D.OverlapCircle(_leftSensor.position, 0.1f);
            if (hit != null && hit.CompareTag("Wall"))
            {
                Debug.Log(hit.name);
                return true;
            }
        }
        if (_horizontalInput > 0)
        {
            var hit = Physics2D.OverlapCircle(_rightSensor.position, 0.1f);
            if (hit != null && hit.CompareTag("Wall"))
            {
                Debug.Log(hit.name);
                return true;
            }
        }

        return false;
    }

    void NormalJump()
    {
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, _jumpVelocity);
            _jumpsRemaining--;
            _fallTimer = 0;

    }

    void LongJump()
    {
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, _jumpVelocity * 1.5f);
            _jumpsRemaining--;
            _fallTimer = 0;
    }

    void WallJump()
    {
        _playerRB.velocity = new Vector2(-3*_horizontalInput * _jumpVelocity, _jumpVelocity * 2.5f);
        _fallTimer = 0;
        _jumpsRemaining--;
    }

    void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("fire");
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (ShouldSlide())
        {
            if (_jumpsRemaining > 0)
            {
                    WallJump();
                return;
            }
        }
        if (_jumpsRemaining > 0 && context.interaction is PressInteraction)
        {
            NormalJump();
        }
        else if (_jumpsRemaining > 0 && context.interaction is HoldInteraction)
        {
            LongJump();
        }
        if (_jumpSound.isPlaying==false)
        {
            _jumpSound.Play();
        }

        // Debug.Log(context.interaction);
    }


}
