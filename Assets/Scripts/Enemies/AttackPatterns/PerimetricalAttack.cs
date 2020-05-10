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

        if(attackData[index].Randomness)
        {
            for (int i = 0; i < attackData[index].ProjectileAmount; i++)
            {
                extraRandomness.Add( new Vector3 (Random.Range(attackData[index].RandomHorizontalFactorMin, attackData[index].RandomHorizontalFactorMax),
                                                  Random.Range(attackData[index].RandomVerticalFactorMin, attackData[index].RandomVerticalFactorMax), 0));
            }
        }
        else
        {
            for (int i = 0; i < attackData[index].ProjectileAmount; i++)
            {
                extraRandomness.Add(Vector3.zero);

            }
        }
            

        if (attackData[index].AttackIsDirectionDependant)
        {
            for(int i = 0; i < attackData[index].ProjectileAmount; i++)
            {
                ProjectileFactory.CreateProjectile(transform.position, attackData[index].Projectile,
                    new Vector3((attackData[index].UniversalSpawnPositionOffset.x + attackData[index].ProjectileSpawnPositionOffset[i].x + extraRandomness[i].x) * transform.right.x,
                    attackData[index].UniversalSpawnPositionOffset.y + attackData[index].ProjectileSpawnPositionOffset[i].y + extraRandomness[i].y, 0),
                    attackData[index].ProjectileSpeed * transform.right.x,
                    attackData[index].ProjectileDestroyDelay, attackData[index].Damage, attackData[index].ProjectileRotations[i]);
            }
        }
        else
        {
            for (int i = 0; i < attackData[index].ProjectileAmount; i++)
            {
                ProjectileFactory.CreateProjectile(transform.position, attackData[index].Projectile, attackData[index].UniversalSpawnPositionOffset + attackData[index].ProjectileSpawnPositionOffset[i] + extraRandomness[i],
                    attackData[index].ProjectileSpeed, attackData[index].ProjectileDestroyDelay, attackData[index].Damage, attackData[index].ProjectileRotations[i]);
            }
        }

    }

}
