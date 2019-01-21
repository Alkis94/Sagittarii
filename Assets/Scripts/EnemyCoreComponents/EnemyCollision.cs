using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyData))]
public class EnemyCollision : MonoBehaviour
{

    private EnemyData enemyData;

    private BoxCollider2D boxCollider;
   
    public delegate void VoidDelegate();
    public event VoidDelegate OnDeath;
    public event VoidDelegate OnGroundCollision;
    public event VoidDelegate OnWallCollision;




    private void Start()
    {
        enemyData = GetComponent<EnemyData>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow")
        {
            enemyData.Health -= PlayerStats.PlayerDamage;
            if (enemyData.Health < 1)
            {
                if (OnDeath != null) OnDeath();
            }
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (OnWallCollision != null) OnGroundCollision();
        }
        if (collision.collider.tag == "Wall")
        {
            if (OnWallCollision != null) OnWallCollision();
        }

    }

    


}
