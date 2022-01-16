using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameState GameState { get; private set; }
    private int bombCount;

    void Start()
    {
        pipeSpawner = GetComponent<PipeSpawner>();
        GameState = GameState.GameStart;
        bombCount = 1;
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
            }
            else
            {
                if (Input.touchCount > 1 && bombCount > 0)
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
        bombCount--;
    }
}
