using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    protected AudioSource audioSource;
    protected Rigidbody2D rigidbody2d;
    protected BoxCollider2D boxCollider2D;
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    private int despawnDelay = 30;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, despawnDelay);
    }

    public void Initialize(Vector3 deadEnemyPosition)
    {
        transform.position = deadEnemyPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PickupPlayerCollision();
        }
    }

    private void PickupPlayerCollision()
    {
        gameObject.layer = 14;
        spriteRenderer.enabled = false;
        audioSource.Play();
        Destroy(gameObject, 0.3f);
    }
}
