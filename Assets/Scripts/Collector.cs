using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    [SerializeField] List<Collectible> _collectibles;
    [SerializeField] UnityEvent _onCollectionComplete;
    static Color _gizmoColor = new Color(0.61f, 0.61f, 0.61f, 1);
    TMP_Text _remainingText;
    int _countCollected;


    private void Start()
    {
        _remainingText = GetComponentInChildren<TMP_Text>();
        foreach (var collectible in _collectibles)
        {
            collectible.onPickedUp += ItemPickedUp;
        }
        _remainingText?.SetText(_collectibles.Count.ToString());
    }

    public void ItemPickedUp()
    {
        _countCollected++;
        int countRemaining = _collectibles.Count - _countCollected;
        _remainingText?.SetText(countRemaining.ToString());


        if (countRemaining > 0)
            return;
        _onCollectionComplete.Invoke();
    }

    //Toplanacak malzemenin ne olduğu ve giriş açılırsa çıkışın otomatik açıldığı bir şekilde scripti düzenle

    //void OnValidate()
    //{
    //    _collectibles = _collectibles.Distinct().ToList();
    //}

    //void OnDrawGizmos()
    //{

    //    foreach (var collectible in _collectibles)
    //    {
    //        if (UnityEditor.Selection.activeGameObject == gameObject)
    //            Gizmos.color = Color.yellow;
    //        else
    //            Gizmos.color = _gizmoColor;
    //        Gizmos.DrawLine(transform.position, collectible.transform.position);
    //    }
    //}
}
