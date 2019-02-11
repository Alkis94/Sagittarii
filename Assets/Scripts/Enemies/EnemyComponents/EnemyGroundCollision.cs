using System.Collections;
using System;
using UnityEngine;


public class EnemyGroundCollision : MonoBehaviour
{

    private BoxCollider2D boxCollider;
   
   
    public event Action OnDeath = delegate { };
    public event Action OnGroundCollision = delegate { };


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            OnGroundCollision?.Invoke();
        }
    }




}
