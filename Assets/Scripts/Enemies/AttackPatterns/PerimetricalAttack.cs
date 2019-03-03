using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PerimetricalAttack : AttackPatern
{
    public GameObject projectile;

    [SerializeField]
    private int damage = 10;

    public override void Attack(EnemyData enemyData)
    {
        if (enemyData.attackIsDirectionDependant)
        {
            foreach (float rotation in enemyData.projectileRotations)
            {
                ProjectileFactory.CreateProjectile(transform.position, projectile, enemyData.projectileSpawnPositionOffset * transform.right.x, enemyData.projectileSpeed * transform.right.x, enemyData.projectileDestroyDelay, damage, rotation);
            }
        }
        else
        {
            foreach (float rotation in enemyData.projectileRotations)
            {
                ProjectileFactory.CreateProjectile(transform.position, projectile, enemyData.projectileSpawnPositionOffset, enemyData.projectileSpeed, enemyData.projectileDestroyDelay, damage, rotation);
            }
        }

    }


}
