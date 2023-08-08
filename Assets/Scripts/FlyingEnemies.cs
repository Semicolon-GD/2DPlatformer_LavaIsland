using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FlyingEnemies : MonoBehaviour
{
    Vector2 _startPosition;
    [SerializeField] Vector2 _direction = Vector2.up;
    [SerializeField] float _maxDistance = 2;
    [SerializeField] float _speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction.normalized*Time.deltaTime*_speed); 

        var distance =Vector2.Distance(_startPosition, transform.position);

        if (distance >= _maxDistance )
        {
            transform.position = _startPosition + (_direction.normalized * _maxDistance);
            _direction *= -1;
        }
    }

    
}
