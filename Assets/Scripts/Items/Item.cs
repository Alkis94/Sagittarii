using UnityEngine;

public class Item : MonoBehaviour
{
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private int despawnDelay = 30;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, despawnDelay);
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
        audioSource.Play();
        Destroy(gameObject, 0.3f);
    }
}
