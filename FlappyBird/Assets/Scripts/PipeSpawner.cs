using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipeBodyPrefab;
    [SerializeField] private GameObject pipeHeadPrefab;

    private const float PIPE_WIDTH = 0.24f;
    private const float CAMERA_SIZE = 1.28f;
    private const float PIPE_HEAD_HEIGHT = 0.12f;

    void Start()
    {
        CreateObstacle(2f, 0.2f, 1f);
    }

    void CreatePipe(float height, float xPosition, bool onBottom)
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

        // Bottom Pipe
        var pipeBody = Instantiate(pipeBodyPrefab, new Vector2(xPosition, bodyYPos), Quaternion.identity);
        pipeBody.transform.localScale = localScale;
        var pipeSprite = pipeBody.GetComponent<SpriteRenderer>();
        pipeSprite.size = new Vector2(PIPE_WIDTH, height);

        // Bottom Head
        var pipeHead = Instantiate(pipeHeadPrefab, new Vector2(xPosition, headYPos), Quaternion.identity);
        pipeHead.transform.localScale = localScale;

        // Bottom Collider adjustment
        var pipeCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeCollider.offset = new Vector2(0f, height / 2);
    }

    void CreateObstacle(float gapPosY, float gapSize, float xPosition)
    {
        CreatePipe(gapPosY - (gapSize / 2), xPosition, true);
        CreatePipe((CAMERA_SIZE * 2) - gapPosY - (gapSize / 2), xPosition, false);
    }
}
