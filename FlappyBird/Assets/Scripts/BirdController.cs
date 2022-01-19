using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField] private float velocity = 1.0f;
    private Rigidbody2D rb;
    private Touch touch;
    public bool dead { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        dead = false;
    }

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