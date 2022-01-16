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
    public GameState GameState { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GameState = GameState.GameStart;
    }

    // Update is called once per frame
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
        }
    }
}
