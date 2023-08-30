using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    public void TakeDamage()
    {
        TakeHit();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player == null)
            return;

        if (collision.contacts[0].normal.y > 0)
        {
            TakeHit();
        }
    }

    void TakeHit()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

    }
}
