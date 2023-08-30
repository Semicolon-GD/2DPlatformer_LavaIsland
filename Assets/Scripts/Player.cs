using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    


    Vector2 _startPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



   

    public void ResetToStart()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public void TeleportTo(Vector2 pos)
    {
        transform.position = pos;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Menu");
    }

}
