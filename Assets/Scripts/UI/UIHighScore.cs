using TMPro;
using UnityEngine;



public class UIHighScore : MonoBehaviour
{
    TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
        ScoreSystem.OnHighScoreChanged += UpdateHighScoreText;
        _text.SetText(PlayerPrefs.GetInt("HighScore").ToString());

    }
    void UpdateHighScoreText(int score)
    {
        _text.SetText(score.ToString());
    }
}