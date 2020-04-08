using UnityEngine;
using System.Collections;

public class EnergyPickup : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().CurrentEnergy += 5;
        }
    }
}
