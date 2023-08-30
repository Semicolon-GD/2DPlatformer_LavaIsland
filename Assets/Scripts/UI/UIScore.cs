using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
        ScoreSystem.OnScoreChanged += UpdateScoreText;
        UpdateScoreText(ScoreSystem.Score);
        //Debug.Log("Score:" + ScoreSystem.Score);
    }

    void OnDestroy()
    {
        ScoreSystem.OnScoreChanged -= UpdateScoreText;
    }

    void UpdateScoreText(int score)
    {
        //Debug.Log(score);
        _text.SetText(score.ToString());
    }

}
