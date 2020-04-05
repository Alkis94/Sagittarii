using Factories;
using UnityEngine;

public class TargetedAttack : AttackPattern
{
    public override void Attack(int index)
    {
        base.Attack(index);
        Vector3 difference;
        float projectileRotation;
        difference = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
        difference.Normalize();
        projectileRotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        ProjectileFactory.CreateProjectile(transform.position,attackData[index].projectile, attackData[index].spawnPositionOffset + attackData[index].projectileSpawnPositionOffset[0], 
            attackData[index].projectileSpeed, attackData[index].projectileDestroyDelay, attackData[index].damage, projectileRotation);
    }
}
