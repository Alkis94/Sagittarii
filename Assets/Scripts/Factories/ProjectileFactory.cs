using UnityEngine;

namespace Factories
{
    public static class ProjectileFactory 
    {
        public static ProjectileDataInitializer CreatePlayerProjectile(AttackInfo attackInfo)
        {
            return CreateProjectile(attackInfo, attackInfo.spawnPosition, UnityLayer.PlayerProjectiles, "PlayerProjectile");
        }

        public static ProjectileDataInitializer CreateEnemyProjectile(AttackInfo attackInfo)
        {
            return CreateProjectile(attackInfo, attackInfo.spawnPosition, UnityLayer.EnemyProjectiles, "EnemyProjectile");
        }

        public static ProjectileDataInitializer CreateExternalEnemyProjectile(AttackInfo attackInfo, Vector3 spawnPosition)
        {
            return CreateProjectile(attackInfo, spawnPosition, UnityLayer.EnemyProjectiles, "EnemyProjectile");
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

