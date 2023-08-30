using System.Collections.Generic;
using UnityEngine;

public class Gem:MonoBehaviour
{
    public static int GemsCollected;
    [SerializeField] List<AudioClip> _clips;


    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player == null)
            return;


        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        UpdateGemAndScore();

        if (_clips.Count > 0)
            GetComponent<AudioSource>().PlayOneShot(_clips[Random.Range(0, _clips.Count)]);
        else
            GetComponent<AudioSource>().Play();


    }

    public static void UpdateGemAndScore()
    {
        GemsCollected++;
        ScoreSystem.Add(100);
    }
}
