using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] Sprite _openMid;
    [SerializeField] Sprite _openTop;

    [SerializeField] SpriteRenderer _rendererMid;
    [SerializeField] SpriteRenderer _rendererTop;
    [SerializeField] EntranceDoor _entrance;


    public void Open()
    {
        
        _rendererMid.sprite = _openMid;
        _rendererTop.sprite = _openTop;
       
    }

}
