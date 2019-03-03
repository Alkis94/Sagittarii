using Factories;
using UnityEngine;

public class TargetedAttack : AttackPatern
{
    public GameObject projectile;

    [SerializeField]
    private int damage = 10;

    public override void Attack(EnemyData enemyData)
    {    
        Vector3 difference;
        float projectileRotation;
        difference = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
        difference.Normalize();
        projectileRotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        ProjectileFactory.CreateProjectile(transform.position,projectile, enemyData.projectileSpawnPositionOffset, enemyData.projectileSpeed, enemyData.projectileDestroyDelay, damage, projectileRotation);
    }
}
