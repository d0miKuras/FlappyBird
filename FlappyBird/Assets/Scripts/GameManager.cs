using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject gameOverWindow;

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
        if (gameOverWindow)
            gameOverWindow.SetActive(false);
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
            if (birdController.dead) // Game Over
            {
                GameState = GameState.GameOver; // Set the game state to prevent pipes from moving
                birdController.SetGravity(false); // Stop bird from moving
                if (gameOverWindow && !gameOverWindowShown) // Show Game Over screen
                {
                    gameOverWindow.SetActive(true);
                    gameOverWindowShown = true;
                    // TODO: Show score in Game Over window
                    if (highScoreManager.IsTopFive(scoreManager.Score)) // If the score is top 5 of high scores, 
                                                                        // let the user know and add it to the list
                    {
                        highScoreManager.AddHighScore(scoreManager.Score);
                        Debug.Log("Score added to the top 5 high scores!"); // TODO: show on screen
                    }
                }
            }
            else
            {
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

    public void RestartLevel()
    {
        SceneManager.LoadScene("GameScene");
    }

}
