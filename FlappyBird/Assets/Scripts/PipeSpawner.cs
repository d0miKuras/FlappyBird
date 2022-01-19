using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    /// <summary>
    /// A pair of obstacle objects.
    /// </summary>
    public class Obstacle
    {
        GameObject topPipe;
        GameObject bottomPipe;
        public bool passed;

        public Obstacle(GameObject _topPipe, GameObject _bottomPipe)
        {
            bottomPipe = _bottomPipe;
            topPipe = _topPipe;
            passed = false;
        }
        public void Move(float speed)
        {
            topPipe.transform.position += new Vector3(-1f, 0f, 0f) * speed * Time.deltaTime;
            bottomPipe.transform.position += new Vector3(-1f, 0f, 0f) * speed * Time.deltaTime;
        }

        public float GetXPos()
        {
            return bottomPipe.transform.position.x;
        }

        public void CleanUp()
        {
            Destroy(topPipe);
            Destroy(bottomPipe);
        }
    }
    [SerializeField] private GameObject pipeBodyPrefab;
    [SerializeField] private GameObject pipeHeadPrefab;
    [SerializeField] private float pipeSpeed = 1.0f;
    [SerializeField] private float xCleanupThreshold = -1.0f;
    [SerializeField] private float gapBetweenObstacles = 2.0f;
    [SerializeField] private float gapBetweenPipes = 1.0f;
    [SerializeField] private float pipeSpawnXPos = 10.0f;

    public bool ObstacleMovement { get; set; }


    private ScoreManager scoreManager;
    private GameManager gameManager;
    public List<Obstacle> ObstacleList { get; private set; }


    private const float PIPE_WIDTH = 0.24f;
    private const float CAMERA_SIZE = 1.28f;
    private const float PIPE_HEAD_HEIGHT = 0.12f;

    void Start()
    {
        ObstacleList = new List<Obstacle>();
        scoreManager = GetComponent<ScoreManager>();
        gameManager = GetComponent<GameManager>();
        CreateObstacle(1.28f, gapBetweenPipes, pipeSpawnXPos);
    }

    void Update()
    {
        HandleObstacleMovement();
        HandleObstacleSpawning();
    }

    /// <summary>
    /// Instantiates a obstacle body and obstacle head with specified height, x-position and the side. Must be parented to an object so that 
    /// no desync happens when moving pipes.
    /// </summary>
    /// <param name="height"></param>
    /// <param name="xPosition"></param>
    /// <param name="onBottom"></param>
    /// <returns>A game object with children being the obstacle head and obstacle body</returns>
    GameObject CreatePipe(float height, float xPosition, bool onBottom)
    {
        float headYPos = -height + CAMERA_SIZE + (PIPE_HEAD_HEIGHT / 2); // set at the top
        float bodyYPos = CAMERA_SIZE;
        Vector3 localScale = new Vector3(1, -1, 1); // local scale for render size
        if (onBottom) // if on the bottom, we need the opposite of those values since the origin is below the center
        {
            headYPos = -headYPos;
            bodyYPos = -bodyYPos;
            localScale = new Vector3(1, 1, 1);
        }

        // Bottom Obstacle
        var bottomPipe = Instantiate(pipeBodyPrefab, new Vector2(xPosition, bodyYPos), Quaternion.identity);
        bottomPipe.transform.localScale = localScale;
        var pipeSprite = bottomPipe.GetComponent<SpriteRenderer>();
        pipeSprite.size = new Vector2(PIPE_WIDTH, height);


        // Bottom Head
        var topPipe = Instantiate(pipeHeadPrefab, new Vector2(xPosition, headYPos), Quaternion.identity);
        topPipe.transform.localScale = localScale;


        // Bottom Collider adjustment
        var pipeCollider = bottomPipe.GetComponent<BoxCollider2D>();
        pipeCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeCollider.offset = new Vector2(0f, height / 2);

        var pipeObj = new GameObject();
        pipeObj.transform.position = bottomPipe.transform.position; // must align positions, otherwise bugs out in cleanup.
        bottomPipe.transform.parent = pipeObj.transform;
        topPipe.transform.parent = pipeObj.transform;

        return pipeObj;
    }

    /// <summary>
    /// Creates a pair of pipes with a specified size gap between them.
    /// </summary>
    /// <param name="gapPosY"></param>
    /// <param name="gapSize"></param>
    /// <param name="xPosition"></param>
    void CreateObstacle(float gapPosY, float gapSize, float xPosition)
    {
        var bottomPipe = CreatePipe(gapPosY - (gapSize / 2), xPosition, true);
        var topPipe = CreatePipe((CAMERA_SIZE * 2) - gapPosY - (gapSize / 2), xPosition, false);
        ObstacleList.Add(new Obstacle(bottomPipe, topPipe));
    }

    /// <summary>
    /// Goes through the list of spawned obstacles and moves them to the left.
    /// </summary>
    void HandleObstacleMovement()
    {
        if (gameManager.GameState == GameState.Playing)
        {
            for (int i = 0; i < ObstacleList.Count; i++)
            {
                var obstacle = ObstacleList[i];
                obstacle.Move(pipeSpeed);

                if (obstacle.GetXPos() < 0.0f && !obstacle.passed) // Score Handling
                {
                    obstacle.passed = true;
                    scoreManager.IncrementScore();
                }

                if (obstacle.GetXPos() < xCleanupThreshold) // Clean up handling
                {
                    CleanupObstacle(obstacle);
                }
            }
        }
    }
    void CleanupObstacle(Obstacle obstacle)
    {
        ObstacleList.Remove(obstacle);
        obstacle.CleanUp();
    }

    void HandleObstacleSpawning()
    {
        var count = ObstacleList.Count;
        if (count < 11)
        {
            if (pipeSpawnXPos - ObstacleList[count - 1].GetXPos() > gapBetweenObstacles)
            {
                var gapPosY = Random.Range(0.56f, (CAMERA_SIZE * 2) - 0.56f); // 0.56 is an experimental value
                CreateObstacle(gapPosY, gapBetweenPipes, pipeSpawnXPos);
            }
        }
    }

    public void DestroyObstaclesOnScreen()
    {
        for (int i = 0; i < ObstacleList.Count; i++)
        {
            // if the obstacle is on the screen, destroy it
            if (ObstacleList[i].GetXPos() > (-CAMERA_SIZE / 2) && ObstacleList[i].GetXPos() < (CAMERA_SIZE / 2))
                CleanupObstacle(ObstacleList[i]);
        }
    }



}
