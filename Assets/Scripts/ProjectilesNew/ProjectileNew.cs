using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileNew : MonoBehaviour
{
   
    private Rigidbody2D rigidbody2d;
    private ProjectileData projectileData;

    private float DestroyDelay;
    private float Speed;
    private Vector2 Direction;
    


    void Start()
    {
        projectileData = GetComponent<ProjectileData>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        rigidbody2d.velocity = new Vector2(Speed * Direction.x, Speed * Direction.y);     
        Destroy(gameObject, DestroyDelay);
    }
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Background")
        {
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.angularVelocity = 0;
            rigidbody2d.isKinematic = true;
            enabled = false;
            Destroy(gameObject, projectileData.ImpactDestroyDelay);
        }
    }

    public void Initialize(Transform enemyPosition, Vector2 direction,Vector3 spawnPositionOffset,float projectileSpeed,float projectileDestroyDelay,float projectileRotation)
    {
        transform.position = enemyPosition.transform.position + spawnPositionOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation);
        Direction = direction;
        Speed = projectileSpeed;
        DestroyDelay = projectileDestroyDelay;
        
    }



}
