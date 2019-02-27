using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PerimetricalAttack : AttackPatern
{
    public GameObject projectile;

    public override void Attack(EnemyData enemyData)
    {
        if (enemyData.attackIsDirectionDependant)
        {
            foreach (float rotation in enemyData.projectileRotations)
            {
                ProjectileFactory.CreateProjectile(transform.position, projectile, enemyData.projectileSpawnPositionOffset * transform.right.x, enemyData.projectileSpeed * transform.right.x, enemyData.projectileDestroyDelay, rotation);
            }
        }
        else
        {
            foreach (float rotation in enemyData.projectileRotations)
            {
                ProjectileFactory.CreateProjectile(transform.position, projectile, enemyData.projectileSpawnPositionOffset, enemyData.projectileSpeed, enemyData.projectileDestroyDelay, rotation);
            }
        }

    }


}
