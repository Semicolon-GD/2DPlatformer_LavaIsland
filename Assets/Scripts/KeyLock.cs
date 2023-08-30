using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyLock : MonoBehaviour
{
    [SerializeField] UnityEvent _onUnlocked;
    public void Unlock()
    {
        
        _onUnlocked.Invoke();
    }
}
