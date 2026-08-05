using UnityEngine;
using System.Collections;

public class Relic : MonoBehaviour
{
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private string relicName = "";
    [SerializeField]
    private string relicDescription = "";
    [SerializeField]
    private RelicRarity relicRarity = RelicRarity.Common;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(MoveUpAndDown());
    }

    public void Initialize(Vector3 deadEnemyPosition)
    {
        transform.position = deadEnemyPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickupPlayerCollision();
        }
    }

    private void PickupPlayerCollision()
    {
        gameObject.layer = UnityLayer.Dead;
        spriteRenderer.enabled = false;
        audioSource.Play();
        UIManager.Instance.ShowItemText(relicName, relicDescription, relicRarity);
        Destroy(transform.parent.gameObject, 0.3f);
    }

    IEnumerator MoveUpAndDown()
    {
        var direction = 1;
        yield return new WaitForSeconds(1f);
        while (true)
        {
            for (var i = 0; i < 20; i++)
            {
                transform.position += direction * Time.fixedDeltaTime * transform.up;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            direction *= -1;
        }
    }
}
