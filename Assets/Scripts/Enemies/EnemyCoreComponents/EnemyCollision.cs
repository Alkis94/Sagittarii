using System.Collections;
using System;
using UnityEngine;


public class EnemyCollision : MonoBehaviour
{

    private EnemyData enemyData;

    private BoxCollider2D boxCollider;
   
   
    public event Action OnDeath = delegate { };
    public event Action OnGroundCollision = delegate { };
    public event Action OnWallCollision = delegate { };




    private void Start()
    {
        enemyData = GetComponent<EnemyData>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow")
        {
            enemyData.Health -= PlayerStats.Damage;
            if (enemyData.Health < 1)
            {
                OnDeath?.Invoke();
            }
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            OnGroundCollision?.Invoke();
        }
        if (collision.collider.tag == "Wall")
        {
            OnWallCollision?.Invoke();
        }

    }

    


}
