using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class PerimetricalAttack : AttackPattern
{
    
    public override void Attack(int index)
    {
        List<Vector3> extraRandomness = new List<Vector3>();
        base.Attack(index);

        if(attackData[index].randomness)
        {
            for (int i = 0; i < attackData[index].projectileAmount; i++)
            {
                extraRandomness.Add( new Vector3 (Random.Range(attackData[index].randomHorizontalFactorMin, attackData[index].randomHorizontalFactorMax),
                                                  Random.Range(attackData[index].randomVerticalFactorMin, attackData[index].randomVerticalFactorMax), 0));
            }
        }
        else
        {
            for (int i = 0; i < attackData[index].projectileAmount; i++)
            {
                extraRandomness.Add(Vector3.zero);

            }
        }
            

        if (attackData[index].attackIsDirectionDependant)
        {
            for(int i = 0; i < attackData[index].projectileAmount; i++)
            {
                ProjectileFactory.CreateProjectile(transform.position, attackData[index].projectile,
                    new Vector3((attackData[index].spawnPositionOffset.x + attackData[index].projectileSpawnPositionOffset[i].x + extraRandomness[i].x) * transform.right.x,
                    attackData[index].spawnPositionOffset.y + attackData[index].projectileSpawnPositionOffset[i].y + extraRandomness[i].y, 0),
                    attackData[index].projectileSpeed * transform.right.x,
                    attackData[index].projectileDestroyDelay, attackData[index].damage, attackData[index].projectileRotations[i]);
            }
        }
        else
        {
            for (int i = 0; i < attackData[index].projectileAmount; i++)
            {
                ProjectileFactory.CreateProjectile(transform.position, attackData[index].projectile, attackData[index].spawnPositionOffset + attackData[index].projectileSpawnPositionOffset[i] + extraRandomness[i],
                    attackData[index].projectileSpeed, attackData[index].projectileDestroyDelay, attackData[index].damage, attackData[index].projectileRotations[i]);
            }
        }

    }

}
