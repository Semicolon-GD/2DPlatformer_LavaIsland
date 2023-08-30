using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICoinsCollected : MonoBehaviour
{
    TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        Coin.CoinsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _text.SetText(Coin.CoinsCollected.ToString());
    }
}
