using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerPrefsText : MonoBehaviour
{
    [SerializeField] string _key;

    void OnEnable()
    {
        int value = PlayerPrefs.GetInt(_key);
        GetComponent<TMP_Text>().SetText(value.ToString());
    }
}
