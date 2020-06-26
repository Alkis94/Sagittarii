using UnityEngine;

namespace Factories
{
    public static class ProjectileFactory 
    {


        public static ProjectileDataInitializer CreateProjectile(Vector3 projectilePosition, GameObject projectile, Vector3 spawnPositionOffset, float projectileSpeed, float projectileDestroyDelay, int damage, Quaternion projectileRotation, ProjectileMovementTypeEnum movementTypeEnum, int layer, string tag)
        {
            var someProjectile = Object.Instantiate(projectile).GetComponent<ProjectileDataInitializer>();
            someProjectile.Initialize(projectilePosition, spawnPositionOffset, projectileSpeed, projectileDestroyDelay, damage, projectileRotation, movementTypeEnum, layer , tag);
            return someProjectile;
        }

        public static ProjectileDataInitializer CreateProjectile(AttackInfo attackInfo, int layer, string tag)
        {
            var someProjectile = Object.Instantiate(attackInfo.projectile).GetComponent<ProjectileDataInitializer>();
            someProjectile.Initialize(attackInfo.spawnPosition, attackInfo.spawnPositionOffset, attackInfo.speed, attackInfo.destroyDelay, attackInfo.damage, attackInfo.rotation, attackInfo.movementTypeEnum,attackInfo.functionMovementType, layer, tag);
            return someProjectile;
        }
    }
}

