using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIWander : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private string barrierTag = "Barrier";

    private Rigidbody2D rb;
    private Vector2 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        movementDirection = Vector2.right;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementDirection * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(barrierTag))
        {
            movementDirection.x *= -1;
        }
    }
}