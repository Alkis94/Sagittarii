using UnityEngine;

namespace Factories
{
    public static class ProjectileFactory 
    {
        public static Projectile CreateProjectile(Vector3 projectilePosition, GameObject projectile, Vector3 spawnPositionOffset, float projectileSpeed, float projectileDestroyDelay, int damage, float projectileRotationZ)
        {
            var someProjectile = Object.Instantiate(projectile).GetComponent<Projectile>();
            someProjectile.Initialize(projectilePosition,  spawnPositionOffset, projectileSpeed, projectileDestroyDelay, damage, projectileRotationZ);
            return someProjectile;
        }

        public static Projectile CreateProjectile(Vector3 projectilePosition, GameObject projectile, Vector3 spawnPositionOffset, float projectileSpeed, float projectileDestroyDelay, int damage, Quaternion projectileRotation)
        {
            var someProjectile = Object.Instantiate(projectile).GetComponent<Projectile>();
            someProjectile.Initialize(projectilePosition, spawnPositionOffset, projectileSpeed, projectileDestroyDelay, damage, projectileRotation);
            return someProjectile;
        }
    }
}
