using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)), ForceMode2D.Impulse);
    }

}
