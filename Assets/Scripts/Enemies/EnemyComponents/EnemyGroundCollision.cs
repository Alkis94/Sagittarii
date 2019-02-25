using System.Collections;
using System;
using UnityEngine;


public class EnemyGroundCollision : MonoBehaviour
{
    public event Action OnGroundCollision = delegate { };


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            OnGroundCollision?.Invoke();
        }
    }
}
