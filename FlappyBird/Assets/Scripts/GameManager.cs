using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    GameStart,
    GamePaused,
    Playing,
    GameOver

}
public class GameManager : MonoBehaviour
{
    [SerializeField] private BirdController birdController;

    private PipeSpawner pipeSpawner;
    private ScoreManager scoreManager;
    private HighScoreManager highScoreManager;
    private UIManager ui;
    public GameState GameState { get; private set; }
    private int bombCount;
    private bool gameOverWindowShown;

    void Start()
    {
        pipeSpawner = GetComponent<PipeSpawner>();
        scoreManager = GetComponent<ScoreManager>();
        highScoreManager = GetComponent<HighScoreManager>();
        ui = GetComponent<UIManager>();
        GameState = GameState.GameStart;
        gameOverWindowShown = false;
        ui.ShowGameOverWindow(false);
    }

    void Update()
    {
        if (birdController)
        {
            if (GameState == GameState.GameStart && Input.touchCount > 0)
            {
                GameState = GameState.Playing;
                birdController.SetGravity(true);
            }
            else if (GameState == GameState.GamePaused)
            {
                birdController.SetGravity(false);
            }
            if (birdController.dead) // Game Over
            {
                GameState = GameState.GameOver; // Set the game state to prevent pipes from moving
                birdController.SetGravity(false); // Stop bird from moving
                if (!gameOverWindowShown) // Show Game Over screen
                {
                    ui.ShowGameOverWindow(true);
                    gameOverWindowShown = true; // To prevent this branch from running multiple times
                    ui.SetScoreText($"SCORE: {scoreManager.Score}");
                    if (highScoreManager.IsTopFive(scoreManager.Score)) // If the score is top 5 of high scores, 
                                                                        // let the user know and add it to the list
                    {
                        highScoreManager.AddHighScore(scoreManager.Score);
                        ui.ShowHighScoreText(true);
                    }
                }
            }
            else if (GameState == GameState.Playing)
            {
                birdController.SetGravity(true);
                if (Input.touchCount > 1 && scoreManager.BombCount > 0)
                {
                    UseBomb();
                }
            }
        }
    }

    void UseBomb()
    {
        Debug.Log("Bomb used");
        pipeSpawner.DestroyObstaclesOnScreen();
        scoreManager.DecrementBomb();
    }

    public void Pause()
    {
        GameState = GameState.GamePaused;
        ui.ShowPauseScreen(true);
    }

    public void Unpause()
    {
        GameState = GameState.Playing;
        ui.ShowPauseScreen(false);
    }
}
