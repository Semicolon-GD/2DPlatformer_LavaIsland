using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Sprite _openMid;
    [SerializeField] Sprite _openTop;

    [SerializeField] SpriteRenderer _rendererMid;
    [SerializeField] SpriteRenderer _rendererTop;
    [SerializeField] Door _exit;
    [SerializeField] Canvas _canvas;

    bool _open;
    Collector _collector;

    void Start()
    {
        _collector = GetComponent<Collector>();
    }


    [ContextMenu("Open Door")]
    public void Open()
    {
        _open = true;
        _rendererMid.sprite = _openMid;
        _rendererTop.sprite = _openTop;
        if (_canvas != null)
            _canvas.enabled = false;
    }

   

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_open == false)
            return;

        var player = collision.GetComponent<Player>();
        if (player != null && _exit != null)
        {
            player.TeleportTo(_exit.transform.position);
        }
    }
}
