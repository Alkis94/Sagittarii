using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PerimetricalAttack : AttackPattern
{
    [SerializeField]
    private List<float> projectileRotations;

    public override void Attack()
    {
        if (attackData.attackIsDirectionDependant)
        {
            foreach (float rotation in projectileRotations)
            {
                ProjectileFactory.CreateProjectile(transform.position, attackData.projectile,
                    attackData.projectileSpawnPositionOffset * transform.right.x, attackData.projectileSpeed * transform.right.x,
                    attackData.projectileDestroyDelay, attackData.damage, rotation);
            }
        }
        else
        {
            foreach (float rotation in projectileRotations)
            {
                ProjectileFactory.CreateProjectile(transform.position, attackData.projectile, attackData.projectileSpawnPositionOffset,
                    attackData.projectileSpeed, attackData.projectileDestroyDelay, attackData.damage, rotation);
            }
        }

    }


}
