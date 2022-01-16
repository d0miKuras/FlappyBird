using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    /// <summary>
    /// A pair of pipe objects.
    /// </summary>
    struct Obstacle
    {
        GameObject topPipe;
        GameObject bottomPipe;

        public Obstacle(GameObject _pipeHead, GameObject _pipeBody)
        {
            bottomPipe = _pipeBody;
            topPipe = _pipeHead;
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


    private List<Obstacle> pipesList;
    private const float PIPE_WIDTH = 0.24f;
    private const float CAMERA_SIZE = 1.28f;
    private const float PIPE_HEAD_HEIGHT = 0.12f;

    void Start()
    {
        pipesList = new List<Obstacle>();
        CreateObstacle(2f, 0.2f, 1f);
        CreateObstacle(2f, 0.2f, 2f);
        CreateObstacle(2f, 0.2f, 3f);
    }

    void Update()
    {
        MovePipes();
    }

    /// <summary>
    /// Instantiates a pipe body and pipe head with specified height, x-position and the side. Must be parented to an object so that 
    /// no desync happens when moving pipes.
    /// </summary>
    /// <param name="height"></param>
    /// <param name="xPosition"></param>
    /// <param name="onBottom"></param>
    /// <returns>A game object with children being the pipe head and pipe body</returns>
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
        pipesList.Add(new Obstacle(bottomPipe, topPipe));
    }

    /// <summary>
    /// Goes through the list of spawned pipes and moves them to the left.
    /// </summary>
    void MovePipes()
    {
        for (int i = 0; i < pipesList.Count; i++)
        {
            var pipe = pipesList[i];
            pipe.Move(pipeSpeed);

            if (pipe.GetXPos() < xCleanupThreshold)
            {
                pipesList.Remove(pipe);
                pipe.CleanUp();
            }
        }
    }


}
