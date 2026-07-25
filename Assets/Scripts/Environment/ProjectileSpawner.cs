using Factories;
using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{

    [SerializeField]
    private float spawnDelayVariation = 0.2f;
    [SerializeField]
    private float projectileSpeedVariation = 0.2f;
    [SerializeField]
    private float spawnChance = 1f;

    public void OnEnable()
    {
        EnemyAttackHandler.OnEnemyExternalAttack += SpawnProjectile;
    }

    public void OnDisable()
    {
        EnemyAttackHandler.OnEnemyExternalAttack -= SpawnProjectile;
    }

    private void SpawnProjectile(AttackInfo attackData)
    {
        if (Random.value <= spawnChance)
        {
            StartCoroutine(SpawnWithDelay(attackData));
        }
    }

    private IEnumerator SpawnWithDelay(AttackInfo attackData)
    {
        var delay = Random.Range(-spawnDelayVariation, spawnDelayVariation);
        attackData.speed += Random.Range(-projectileSpeedVariation, projectileSpeedVariation);
        yield return new WaitForSeconds(delay);
        ProjectileFactory.CreateExternalEnemyProjectile(attackData, transform.position);
    }
}

