using UnityEngine;
using System.Collections;

public class BatFangs : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().LifeSteal += 3;
            collision.GetComponent<PlayerStats>().LifeStealChance += 0.1f;
        }
    }
}
