using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverWindow;

    void Awake()
    {
        if (gameOverWindow)
        {
            ShowHighScoreText(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowGameOverWindow(bool value)
    {
        if (gameOverWindow)
            gameOverWindow.SetActive(value);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SetScoreText(string _text)
    {
        if (gameOverWindow)
        {
            gameOverWindow.transform.Find("ScoreText").GetComponent<Text>().text = _text;
        }
    }

    public void ShowHighScoreText(bool value)
    {
        if (gameOverWindow)
        {
            gameOverWindow.transform.Find("NewHighScore").gameObject.SetActive(value);
        }
    }
}
