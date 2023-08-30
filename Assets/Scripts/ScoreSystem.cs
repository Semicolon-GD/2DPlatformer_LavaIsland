using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnHighScoreChanged;

    public static int Score { get; private set; }

    static int _highScore;


    void Start()
    {
        _highScore = PlayerPrefs.GetInt("HighScore");
        Score = 0;
    }

    public static void Add(int points)
    {
        Score += points;
        if (Score > _highScore)
        {
            _highScore = Score;
            OnHighScoreChanged.Invoke(_highScore);
            PlayerPrefs.SetInt("HighScore", _highScore);
        }
        OnScoreChanged.Invoke(Score);
    }
}
