using UnityEngine;

public class Pickup : MonoBehaviour
{
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Vector3 deadEnemyPosition)
    {
        transform.position = deadEnemyPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PickupPlayerCollision();
        }
    }

    private void PickupPlayerCollision()
    {
        gameObject.layer = 14;
        spriteRenderer.enabled = false;
        audioSource.enabled = true;
        audioSource.Play();
        Destroy(gameObject, 0.3f);
    }
}
