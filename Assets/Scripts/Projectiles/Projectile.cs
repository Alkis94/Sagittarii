using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float DestroyDelay { private set; get; }
    public float Speed { private set; get; }


    private void Start()
    {
        Destroy(gameObject, DestroyDelay);
    }

    public void Initialize(Vector3 parentPosition,Vector3 spawnPositionOffset,float projectileSpeed,float projectileDestroyDelay,float projectileRotation)
    {
        transform.position = parentPosition + spawnPositionOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation);
        Speed = projectileSpeed;
        DestroyDelay = projectileDestroyDelay;
    }

    public void Initialize(Vector3 parentPosition, Vector3 spawnPositionOffset, float projectileSpeed, float projectileDestroyDelay, Quaternion projectileRotation)
    {
        transform.position = parentPosition + spawnPositionOffset;
        transform.rotation = projectileRotation;
        Speed = projectileSpeed;
        DestroyDelay = projectileDestroyDelay;
    }

}
