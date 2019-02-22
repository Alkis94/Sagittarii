using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePickup : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStats.Damage += Random.Range(0,6);
            //Debug.Log("Player Damage : " + PlayerStats.Damage);
        }
    }

}
