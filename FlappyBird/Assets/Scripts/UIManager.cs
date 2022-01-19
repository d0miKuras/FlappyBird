using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverWindow;
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject leaderboard;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject counters;

    private HighScoreManager highScoreManager;

    void Awake()
    {
        highScoreManager = GetComponent<HighScoreManager>();
        ShowHighScoreText(false);
        ShowMainMenuButtons();
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

    public void ShowMainMenuButtons()
    {
        if (mainMenuButtons && leaderboard)
        {
            mainMenuButtons.SetActive(true);
            leaderboard.SetActive(false);
        }
    }

    public void ShowLeaderboard()
    {
        if (mainMenuButtons && leaderboard)
        {
            mainMenuButtons.SetActive(false);
            UpdateLeaderboard();
            leaderboard.SetActive(true);
        }
    }

    public void ShowPauseScreen(bool value)
    {
        if (pauseScreen && counters)
        {
            pauseScreen.SetActive(value);
            counters.SetActive(!value);
        }
    }

    private void UpdateLeaderboard()
    {
        if (leaderboard && highScoreManager)
        {
            var text = leaderboard.transform.Find("HighScoreContainer").Find("HighScoreList").GetComponent<Text>();

            var list = highScoreManager.GetHighscores();
            if (text && list != null)
            {
                text.text = "";
                foreach (var score in list.highscoreList)
                {
                    text.text += $"{score.score}\n";
                }
            }
        }
    }
}
