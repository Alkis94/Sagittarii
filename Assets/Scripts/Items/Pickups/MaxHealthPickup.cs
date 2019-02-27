using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPickup : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats.MaximumHealth += 10;
            PlayerStats.CurrentHealth += 10;
        }
    }

}
