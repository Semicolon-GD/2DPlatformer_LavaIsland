using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartGameButton : MonoBehaviour
{
    [SerializeField] string _levelName;

    public string LevelName => _levelName;

    public void LoadLevel()
    {
        SceneManager.LoadScene(_levelName);
    }
}
