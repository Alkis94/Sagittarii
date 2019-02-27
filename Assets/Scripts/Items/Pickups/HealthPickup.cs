using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats.CurrentHealth += 10;
            //PlayerStats.ChangePlayerCurrentHealth(PlayerStats.currentHealth + 10);
        }
    }
}
