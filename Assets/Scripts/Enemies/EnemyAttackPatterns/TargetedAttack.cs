using Factories;
using UnityEngine;

public class TargetedAttack : AttackPatern
{

    public GameObject projectile;

    public override void Attack(EnemyData enemyData)
    {    
        Vector3 diff;
        float projectileRotation;
        diff = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
        diff.Normalize();
        projectileRotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        Vector2 Direction;
        Direction = ((Vector2)GameObject.FindGameObjectWithTag("Player").transform.position - (Vector2)transform.position).normalized;
        ProjectileFactory.CreateProjectile(transform.position,projectile, enemyData.ProjectileSpawnPositionOffset, enemyData.ProjectileSpeed, enemyData.ProjectileDestroyDelay,projectileRotation);
    }

    
}
