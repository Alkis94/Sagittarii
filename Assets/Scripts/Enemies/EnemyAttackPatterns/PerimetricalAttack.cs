using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PerimetricalAttack : AttackPatern
{
    private EnemyMovement enemyMovement;
    public GameObject projectile;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public override void Attack(EnemyData enemyData)
    {
        if (enemyData.AttackIsDirectionDependant)
        {
            foreach (float rotation in enemyData.projectileRotations)
            {
                ProjectileFactory.CreateProjectile(transform.position, projectile, enemyData.ProjectileSpawnPositionOffset, enemyData.ProjectileSpeed, enemyData.ProjectileDestroyDelay, rotation);
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
