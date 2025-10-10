using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Target; // Target to follow (Kawsarin)
    public float detectionRadius = 5f;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, Target.position);
        if (distance < detectionRadius)
        {
            Vector2 direction = (Target.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0);
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        // Usar velocity para que la gravedad funcione correctamente
        rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
