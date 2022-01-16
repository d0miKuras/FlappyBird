using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField] private float velocity = 1.0f;
    private Rigidbody2D rb;
    private Touch touch;
    public bool dead { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !dead)
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
        dead = true;
    }

    public void SetGravity(bool val)
    {
        rb.simulated = val;
    }
}
