using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;
    [SerializeField]
    private CoinType coinType = CoinType.copper;

    public const int copperValue = 5;
    public const int silverValue = 10;
    public const int goldValue = 100;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        rigidbody2d.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 8f)), ForceMode2D.Impulse);
        Invoke("EnableCollider", 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().Gold += (int)coinType;
        }
    }

    private void EnableCollider()
    {
        boxCollider2d.enabled = true;
    }

}
