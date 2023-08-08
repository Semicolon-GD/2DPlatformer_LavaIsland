using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    [SerializeField][Range(1, 2)] int _playerNumber;
    [SerializeField] Transform _feet;
    [SerializeField] Transform _leftSensor;
    [SerializeField] Transform _rightSensor;
    [SerializeField] Vector2 _startPosition;
    [Header("Movement")]
    [SerializeField] float _speed=2;
    [SerializeField] float _slipFactor=.3f;
    [SerializeField] float _acceleration=10;
    [SerializeField] float _breaking=10;
    [Header("Jump")]
    [SerializeField] float _jumpVelocity=8;
    [SerializeField] int _maxJumps=2;
    [SerializeField] float _downPull=.1f;
    [SerializeField] float _maxJumpDuration=.2f;
    [SerializeField] float _wallSlideSpeed=1f;
    

    [SerializeField] float _airAcceleration=2;
    [SerializeField] float _airBreaking=0;

    Rigidbody2D _playerRB;
    PlayerInput _playerInput;
    PlayerInputActions _playerInputActions;
    int _layerMask;
    SpriteRenderer _spriteRendeder;
    Vector2 _movementVector;
    float _horizontalInput;
    float _horizontalMovement;
    float _refVelocity;
   
    bool _isGrounded;
    bool _isOnSlipperySurface;
    int _jumpsRemaining;
    float _fallTimer=0;
    float _jumpTimer;
    

    void Awake()
    {
        
        _playerRB = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _layerMask = LayerMask.GetMask("Default", "Mushroom");
        _spriteRendeder=GetComponent<SpriteRenderer>();
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
        //UpdateIsGrounded();
        //ReadWalkingInput();
        //JumpTimerControll();

        //UpdateSpriteDirection();
        UpdateIsGrounded();
        ReadWalkingInput();
        if (_isOnSlipperySurface)
            SlipperyWalk();
        else
            Walk();

       // UpdateAnimator();

        UpdateSpriteDirection();

        //if (ShouldSlide())
        //{
        //    if (ShouldStartJump())
        //        WallJump();
        //    else
        //        Slide();
        //    return;
        //}
        //if (ShouldStartJump())
        //    JumpMovement();
        //else if (ShouldContinueJump())
        //    ContinueJump();
        JumpTimerControll();
        ManageFall();
    }

    private void JumpTimerControll()
    {
        _jumpTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_isOnSlipperySurface)
            SlipperyWalk();
        else
            Walk();
        if (ShouldSlide())
        {
            Slide();
            return;
        }

        ManageFall();
    }

    private void ManageFall()
    {
        if (_isGrounded && _fallTimer > 0.1f)
        {
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;
        }
        else
        {
            _fallTimer += Time.deltaTime;
            var downForce = _downPull * _fallTimer * _fallTimer;
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, _playerRB.velocity.y - downForce);
            // Debug.Log(downForce);

          //  Debug.Log("ff" + _playerRB.velocity + _fallTimer +_isGrounded) ;
        }
    }

    private void UpdateSpriteDirection()
    {
        if (_horizontalInput != 0)
            _spriteRendeder.flipX = _horizontalInput < 0;
    }

    private void UpdateIsGrounded()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, .1f, _layerMask);

        _isGrounded = hit != null;

        _isOnSlipperySurface = hit?.CompareTag("Slippery") ?? false;

    }

   

   
    private void ReadWalkingInput()
    {
        _horizontalInput = _playerInputActions.Player1.Movement.ReadValue<Vector2>().x * _speed;
    }

    private void Walk()
    {
        _refVelocity = _playerRB.velocity.x;
        float smoothnessMultiplier = _horizontalInput == 0 ? _breaking : _acceleration;
        if (_isGrounded == false)
            smoothnessMultiplier = _horizontalInput == 0 ? _airBreaking : _airAcceleration;

        _horizontalMovement = Mathf.Lerp(_playerRB.velocity.x, _horizontalInput, Time.deltaTime);
        //_horizontalMovement = Mathf.SmoothDamp(_horizontalMovement, _horizontalInput * _speed,ref _refVelocity,_slipFactor/smoothnessMultiplier);
        Debug.Log(_horizontalMovement);
        _playerRB.velocity = new Vector2(_horizontalMovement, _playerRB.velocity.y);
    }

    

    private void SlipperyWalk()
    {
        _horizontalMovement = Mathf.SmoothDamp(_horizontalMovement, _horizontalInput * _speed, ref _refVelocity, _slipFactor*3);
        _playerRB.velocity = new Vector2(_horizontalMovement, _playerRB.velocity.y);

    }

    public void Jump(InputAction.CallbackContext context)
    {
        //if (ShouldStartJump())
        //    JumpMovement();
        //else if (ShouldContinueJump())
        //    ContinueJump();
        if (ShouldSlide())
        {
            if (ShouldStartJump())
                WallJump();
            //else
            //    Slide();
            return;
        }
        if (ShouldStartJump())
            JumpMovement();
        else if (ShouldContinueJump())
            ContinueJump();


    }

    private bool ShouldSlide()
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
               // Debug.Log(hit.name);
                return true;
            }
        }
        if (_horizontalInput > 0)
        {
            var hit = Physics2D.OverlapCircle(_rightSensor.position, 0.1f);
            if (hit != null && hit.CompareTag("Wall"))
            {
             //   Debug.Log(hit.name);
                return true;
            }
        }

        return false;
    }

    private void WallJump()
    {
        Debug.Log(_playerRB.velocity);
        _playerRB.velocity = new Vector2(-_horizontalInput * _jumpVelocity, _jumpVelocity * 1.5f);
        Debug.Log(_playerRB.velocity);
        Debug.Log(_playerRB.velocity);
    }

    private void Slide()
    {
        _playerRB.velocity = new Vector2(_playerRB.velocity.x, -_wallSlideSpeed);
        Debug.Log(_playerRB.velocity);
    }

    private void ContinueJump()
    {
        _playerRB.velocity = new Vector2(_playerRB.velocity.x, _jumpVelocity);
        _fallTimer = 0;
    }

    private void JumpMovement()
    {
        _playerRB.velocity = new Vector2(_playerRB.velocity.x, _jumpVelocity);
        _jumpsRemaining--;
        _fallTimer = 0;
        _jumpTimer = 0;
    }

    private bool ShouldStartJump()
    {
        if (_jumpsRemaining > 0)
            return true;
        else
            return false;
    }

    private bool ShouldContinueJump()
    {
        if (_jumpTimer <= _maxJumpDuration)
            return true;
        else 
            return false;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire "+ context.phase);
    }


    internal void LooseLife()
    {
        //SceneManager.LoadScene("Menu");
        _playerRB.position = _startPosition;

    }
}
