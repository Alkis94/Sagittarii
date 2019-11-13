using UnityEngine;
using System.Collections;

public class GoldPickup : CoinPickup
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().Gold += 10;
        }
    }
}
