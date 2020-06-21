using UnityEngine;
using Factories;
using System;

public class Trident : MonoBehaviour
{
    private void FireArrowWithTrident(GameObject arrowEmitter,GameObject projectile,float projectileSpeed, float projectileDestroyDelay, float arrowPower, int damage)
    {
        ProjectileFactory.CreateProjectile(arrowEmitter.transform.position, projectile, Vector3.zero, projectileSpeed * arrowPower, projectileDestroyDelay, damage, Quaternion.Euler(0, 0, arrowEmitter.transform.rotation.eulerAngles.z + 10), ProjectileMovementTypeEnum.arrow);
        ProjectileFactory.CreateProjectile(arrowEmitter.transform.position, projectile, Vector3.zero, projectileSpeed * arrowPower, projectileDestroyDelay, damage, arrowEmitter.transform.rotation, ProjectileMovementTypeEnum.arrow);
        ProjectileFactory.CreateProjectile(arrowEmitter.transform.position, projectile, Vector3.zero, projectileSpeed * arrowPower, projectileDestroyDelay, damage, Quaternion.Euler(0, 0, arrowEmitter.transform.rotation.eulerAngles.z - 10), ProjectileMovementTypeEnum.arrow);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponentInChildren<PlayerFireProjectile>().FireArrow = FireArrowWithTrident;
            RelicFactory.PlayerHasUniqueRelic["Trident"] = true;
        }
    }
}
