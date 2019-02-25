using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats.Damage += Random.Range(0, 6);
            //Debug.Log("Player Damage : " + PlayerStats.Damage);
        }
    }

}
