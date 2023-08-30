using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] KeyLock _keyLock;
    AudioSource _audioSource;

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
        {
            transform.SetParent(player.transform);
            transform.localPosition = Vector3.up;
            if (_audioSource != null)
                _audioSource.Play();
        }

        var keyLock = collision.GetComponent<KeyLock>();
        if (keyLock != null && keyLock == _keyLock)
        {
            keyLock.Unlock();
            Destroy(gameObject);
        }
    }
}
