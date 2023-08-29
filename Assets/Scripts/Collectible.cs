using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public event Action onPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        //gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        if (onPickedUp != null)
            onPickedUp?.Invoke();

        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.Play();


    }
}
