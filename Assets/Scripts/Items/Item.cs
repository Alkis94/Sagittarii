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

    public void Initialize(Transform deadEnemyPosition)
    {
        transform.position = deadEnemyPosition.transform.position;
    }

    protected void PickupPlayerCollision()
    {
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        audioSource.Play();
        Destroy(gameObject,0.3f);
    }

    protected void PickupGroundCollision()
    {
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.angularVelocity = 0;
        rigidbody2d.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PickupPlayerCollision();
        }
        else if (collision.gameObject.tag == "Ground")
        {
            PickupGroundCollision();
        }
    }

}
