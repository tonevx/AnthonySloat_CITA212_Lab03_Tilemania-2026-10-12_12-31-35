using UnityEngine;

public class BulletLogic : MonoBehaviour
{
   [SerializeField] Rigidbody2D rb;
   [SerializeField] float bulletSpeed = 20f;

   float xSpeed;
   Movement player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<Movement>(); //again, FindFirstObjectByType is deprecated.
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 1f);
    }
}
