using Factories;
using UnityEngine;

public class TargetedAttack : AttackPattern
{
    public override void Attack()
    {    
        Vector3 difference;
        float projectileRotation;
        difference = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
        difference.Normalize();
        projectileRotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        ProjectileFactory.CreateProjectile(transform.position,attackData.projectile, attackData.projectileSpawnPositionOffset, attackData.projectileSpeed, attackData.projectileDestroyDelay, attackData.damage, projectileRotation);
    }
}
