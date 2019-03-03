using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float DestroyDelay { private set; get; }
    public float Speed { private set; get; }
    public int Damage { private set; get; }


    private void Start()
    {
        Destroy(gameObject, DestroyDelay);
    }

    public void Initialize(Vector3 parentPosition,Vector3 spawnPositionOffset,float projectileSpeed,float projectileDestroyDelay,int damage,float projectileRotation)
    {
        transform.position = parentPosition + spawnPositionOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation);
        Speed = projectileSpeed;
        DestroyDelay = projectileDestroyDelay;
        Damage = damage;
    }

    public void Initialize(Vector3 parentPosition, Vector3 spawnPositionOffset, float projectileSpeed, float projectileDestroyDelay, int damage, Quaternion projectileRotation)
    {
        transform.position = parentPosition + spawnPositionOffset;
        transform.rotation = projectileRotation;
        Speed = projectileSpeed;
        DestroyDelay = projectileDestroyDelay;
        Damage = damage;
    }

}
