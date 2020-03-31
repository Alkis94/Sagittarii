using UnityEngine;
using System.Collections;

public class EnergyPickup : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStats>().CurrentEnergy += 5;
        }
    }
}
