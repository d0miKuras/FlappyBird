using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField] private float velocity = 1.0f;
    private Rigidbody2D rb;
    private Touch touch;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided");
    }
}
