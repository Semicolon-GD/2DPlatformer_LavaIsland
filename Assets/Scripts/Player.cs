using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int _maxHP=3;


    Vector2 _startPosition;
    int _currentHP;

    // Start is called before the first frame update
    void Start()
    {
        _currentHP = _maxHP;
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    internal void LooseHP()
    {
        _currentHP--;
        if (_currentHP <= 0)
            GameOver();
        else
            ResetToStart();

    }

    void ResetToStart()
    {
        transform.position= _startPosition;
    }

    void GameOver()
    {
        Debug.Log("Game Over");
        ResetToStart();
        _currentHP = _maxHP;
    }
}
