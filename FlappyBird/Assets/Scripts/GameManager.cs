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
    public GameState gameState { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (birdController)
        {
            if (gameState == GameState.GameStart && Input.touchCount > 0)
            {
                gameState = GameState.Playing;
                birdController.EnableGravity();
            }
            if (birdController.dead)
            {
                gameState = GameState.GameOver;
            }
        }
    }
}
