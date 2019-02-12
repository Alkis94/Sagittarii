using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PerimetricalAttack : AttackPatern
{
    public GameObject projectile;

    public override void Attack(EnemyData enemyData)
    {
        if (enemyData.AttackIsDirectionDependant)
        {
            foreach (float rotation in enemyData.projectileRotations)
            {
                ProjectileFactory.CreateProjectile(transform.position, projectile, enemyData.ProjectileSpawnPositionOffset * transform.right.x, enemyData.ProjectileSpeed * transform.right.x, enemyData.ProjectileDestroyDelay, rotation);
            }
        }
        else
        {
            foreach (float rotation in enemyData.projectileRotations)
            {
                ProjectileFactory.CreateProjectile(transform.position, projectile, enemyData.ProjectileSpawnPositionOffset, enemyData.ProjectileSpeed, enemyData.ProjectileDestroyDelay, rotation);
            }
        }

    }


}
