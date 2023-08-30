using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static int CoinsCollected;
    [SerializeField] List<AudioClip> _clips;


    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player == null)
            return;


        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        UpdateCoinAndScore();

        if (_clips.Count > 0)
            GetComponent<AudioSource>().PlayOneShot(_clips[Random.Range(0, _clips.Count)]);
        else
            GetComponent<AudioSource>().Play();


    }

    public static void UpdateCoinAndScore()
    {
        CoinsCollected++;
        ScoreSystem.Add(100);
    }
}
