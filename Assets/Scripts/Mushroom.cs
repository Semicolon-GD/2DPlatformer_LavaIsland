using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] float _bounceVelocity = 10;
    AudioSource _audioSource;

    private void Awake() => _audioSource = GetComponent<AudioSource>();
    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            var rigidbody2D = player.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, _bounceVelocity);
                if (_audioSource != null)
                    _audioSource.Play();
            }
        }
    }
}
