using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToogleSwitch : MonoBehaviour
{
    [SerializeField] ToggleDirection _startDirection = ToggleDirection.Center;

    [SerializeField] Sprite _toggleLeft;
    [SerializeField] Sprite _toggleRight;
    [SerializeField] Sprite _toggleCenter;

    [SerializeField] UnityEvent _onLeft;
    [SerializeField] UnityEvent _onRight;
    [SerializeField] UnityEvent _onCenter;

    [SerializeField] AudioClip _leftSound;
    [SerializeField] AudioClip _rightSound;

    SpriteRenderer _spriteRenderer;
    ToggleDirection _currentDirection;
    AudioSource _audioSource;

    enum ToggleDirection
    {
        Left,
        Center,
        Right,
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetToogleDirection(_startDirection, true);
        _audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        var playerRigidbody = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody == null)
            return;

        bool wasOnRight = collision.transform.position.x > transform.position.x;
        bool playerWalkingRight = playerRigidbody.velocity.x > 0;
        bool playerWalkingLeft = playerRigidbody.velocity.x < 0;

        if (wasOnRight && playerWalkingRight)
            SetToogleDirection(ToggleDirection.Right);
        else if (wasOnRight == false && playerWalkingLeft)
            SetToogleDirection(ToggleDirection.Left);

    }

    void SetToogleDirection(ToggleDirection direction, bool force = false)
    {
        if (force == false && _currentDirection == direction)
            return;

        _currentDirection = direction;
        switch (direction)
        {
            case ToggleDirection.Left:
                _spriteRenderer.sprite = _toggleLeft;
                _onLeft.Invoke();
                if (_audioSource != null)
                    _audioSource.PlayOneShot(_leftSound);
                break;
            case ToggleDirection.Center:
                _spriteRenderer.sprite = _toggleCenter;
                _onCenter.Invoke();
                break;
            case ToggleDirection.Right:
                _spriteRenderer.sprite = _toggleRight;
                _onRight.Invoke();
                if (_audioSource != null)
                    _audioSource.PlayOneShot(_rightSound);
                break;
            default:
                break;
        }

    }

    //void OnValidate()
    //{
    //    if (_spriteRenderer == null)
    //        _spriteRenderer = GetComponent<SpriteRenderer>();

    //    ToggleDirection direction = _startDirection;
    //    switch (_startDirection)
    //    {
    //        case ToggleDirection.Left:
    //            _spriteRenderer.sprite = _toggleLeft;
    //            break;
    //        case ToggleDirection.Center:
    //            _spriteRenderer.sprite = _toggleCenter;
    //            break;
    //        case ToggleDirection.Right:
    //            _spriteRenderer.sprite = _toggleRight;
    //            break;
    //        default:
    //            break;
    //    }
    //}

}
