using UnityEngine;

namespace Factories
{
    public static class ProjectileFactory 
    {
        public static ProjectileDataInitializer CreatePlayerProjectile(AttackInfo attackInfo)
        {
            return CreateProjectile(attackInfo, attackInfo.spawnPosition, 11, "PlayerProjectile");
        }

        public static ProjectileDataInitializer CreateEnemyProjectile(AttackInfo attackInfo)
        {
            return CreateProjectile(attackInfo, attackInfo.spawnPosition, 13, "EnemyProjectile");
        }

        public static ProjectileDataInitializer CreateExternalEnemyProjectile(AttackInfo attackInfo, Vector3 spawnPosition)
        {
            return CreateProjectile(attackInfo, spawnPosition, 13, "EnemyProjectile");
        }

        private static ProjectileDataInitializer CreateProjectile(AttackInfo attackInfo, Vector3 spawnPosition, int layer, string tag)
        {
            var someProjectile = Object.Instantiate(attackInfo.projectile).GetComponent<ProjectileDataInitializer>();
            var spawnInfo = new ProjectileSpawnInfo
            {
                spawnPosition = spawnPosition,
                projectile = attackInfo.projectile,
                spawnPositionOffset = attackInfo.spawnPositionOffset,
                speed = attackInfo.speed,
                destroyDelay = attackInfo.destroyDelay,
                damage = attackInfo.damage,
                rotation = attackInfo.rotation,
                movementTypeEnum = attackInfo.movementTypeEnum,
                functionMovementType = attackInfo.functionMovementType
            };
            someProjectile.Initialize(spawnInfo, layer, tag);
            return someProjectile;
        }
    }
}

