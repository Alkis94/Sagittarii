using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPickup : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().MaximumHealth += 10;
            collision.GetComponent<PlayerStats>().CurrentHealth += 10;
        }
    }

}
