using System;
using UnityEngine;

public abstract class HittableFromBelow : MonoBehaviour
{
    [SerializeField] protected Sprite _usedSprite;
    Animator _animator;
    AudioSource _audioSource;

    protected virtual bool CanUse => true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (CanUse == false)
            return;
        var player = collision.collider.GetComponent<Player>();
        if (player == null)
            return;
        if (collision.contacts[0].normal.y > 0)
        {
            PlayAudio();
            PlayAnimation();
            Use();

            if (CanUse == false)
                GetComponent<SpriteRenderer>().sprite = _usedSprite;
        }
    }

    void PlayAudio()
    {
        if (_audioSource != null)
            _audioSource.Play();
    }

    void PlayAnimation()
    {
        if (_animator != null)
            _animator.SetTrigger("Used");
    }

    protected abstract void Use();

}
