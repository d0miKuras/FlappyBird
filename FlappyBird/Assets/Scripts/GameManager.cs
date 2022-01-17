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
    private UIManager ui;
    public GameState GameState { get; private set; }
    private int bombCount;
    private bool gameOverWindowShown;

    void Start()
    {
        pipeSpawner = GetComponent<PipeSpawner>();
        scoreManager = GetComponent<ScoreManager>();
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
            if (birdController.dead)
            {
                GameState = GameState.GameOver;
                birdController.SetGravity(false);
                if (gameOverWindow && !gameOverWindowShown)
                {
                    gameOverWindow.SetActive(true);
                    gameOverWindowShown = true;
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
