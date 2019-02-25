using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    protected AudioSource audioSource;
    protected Rigidbody2D rigidbody2d;
    protected BoxCollider2D boxCollider2D;
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    private int DespawnDelay = 30;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, DespawnDelay);
    }

    public void Initialize(Vector3 deadEnemyPosition)
    {
        transform.position = deadEnemyPosition;
    }

    protected void PickupPlayerCollision()
    {
        spriteRenderer.enabled = false;
        audioSource.Play();
        Destroy(gameObject,0.3f);
    }

    protected void PickupGroundCollision()
    {
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PickupPlayerCollision();
        }
        else if (collision.tag == "Ground")
        {
            PickupGroundCollision();
        }
    }
}
