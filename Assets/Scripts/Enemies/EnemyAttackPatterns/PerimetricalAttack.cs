using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerimetricalAttack : AttackPatern
{
    private EnemyMovement enemyMovement;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public override void Attack(EnemyData enemyData)
    {
        if(enemyData.AttackIsDirectionDependant)
        {
            foreach (Vector2 direction in enemyData.projectileDirections)
            {
                //ObjectFactory.Instance.CreateProjectileNew(transform, direction*(-enemyMovement.HorizontalDirection), enemyData.projectile, enemyData.ProjectileSpawnPositionOffset * (-enemyMovement.HorizontalDirection), enemyData.ProjectileSpeed, enemyData.ProjectileDestroyDelay, 0);
            }
        }
        else
        {
            foreach (Vector2 direction in enemyData.projectileDirections)
            {
                //ObjectFactory.Instance.CreateProjectileNew(transform, direction, enemyData.projectile, enemyData.ProjectileSpawnPositionOffset, enemyData.ProjectileSpeed, enemyData.ProjectileDestroyDelay, 0);
            }
        }
        
    }


}
