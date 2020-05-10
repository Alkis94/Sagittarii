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

        ProjectileFactory.CreateProjectile(transform.position,attackData[index].Projectile, attackData[index].UniversalSpawnPositionOffset + attackData[index].ProjectileSpawnPositionOffset[0], 
            attackData[index].ProjectileSpeed, attackData[index].ProjectileDestroyDelay, attackData[index].Damage, projectileRotation);
    }
}
