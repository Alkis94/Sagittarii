using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    [SerializeField]
    private CoinType coinType = CoinType.copper;

    public const int copperValue = 5;
    public const int silverValue = 10;
    public const int goldValue = 20;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().Gold += (int)coinType;
        }
    }

}
