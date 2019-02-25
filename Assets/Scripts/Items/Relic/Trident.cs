using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class Trident : MonoBehaviour
{
    private void FireArrowWithTrident(GameObject arrowEmitter,GameObject projectile,float projectileSpeed, float projectileDestroyDelay, float arrowPower)
    {
        ProjectileFactory.CreateProjectile(arrowEmitter.transform.position, projectile, Vector3.zero, projectileSpeed * arrowPower, projectileDestroyDelay, Quaternion.Euler(0, 0, arrowEmitter.transform.rotation.eulerAngles.z + 10));
        ProjectileFactory.CreateProjectile(arrowEmitter.transform.position, projectile, Vector3.zero, projectileSpeed * arrowPower, projectileDestroyDelay, arrowEmitter.transform.rotation);
        ProjectileFactory.CreateProjectile(arrowEmitter.transform.position, projectile, Vector3.zero, projectileSpeed * arrowPower, projectileDestroyDelay, Quaternion.Euler(0, 0, arrowEmitter.transform.rotation.eulerAngles.z - 10));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponentInChildren<PlayerFireProjectile>().FireArrow = FireArrowWithTrident;
        }
    }
}
