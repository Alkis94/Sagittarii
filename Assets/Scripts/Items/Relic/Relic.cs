using UnityEngine;
using System.Collections;

public class Relic : MonoBehaviour
{
    private AudioSource audioSource;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;

    [SerializeField]
    private int despawnDelay = 60;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponentInParent<Rigidbody2D>();
        StartCoroutine(MoveUpAndDOwn());
        Destroy(transform.parent.gameObject, despawnDelay);
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
        Destroy(transform.parent.gameObject, 0.3f);
    }

    IEnumerator MoveUpAndDOwn()
    {
        int direction = 1;
        yield return new WaitForSeconds(1f);
        while(true)
        {
            for(int i = 0; i < 20; i++)
            {
                transform.position += transform.up * Time.fixedDeltaTime * direction;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            direction *= -1;
        }
    }
}
