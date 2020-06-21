using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class AttackPattern : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        
        GetComponent<EnemyStats>().EnemyDied += StopCoroutines;
    }

    private void OnDisable()
    {
        GetComponent<EnemyStats>().EnemyDied -= StopCoroutines;
    }

    public void Attack(AttackData attackData)
    {
        if (attackData.AttackSound != null)
        {
            audioSource.PlayOneShot(attackData.AttackSound);
        }

        StartCoroutine(StartAttacking(attackData));
    }

    IEnumerator StartAttacking(AttackData attackData)
    {
        for (int j = 0; j < attackData.ConsecutiveAttacks; j++)
        {
            for (int i = 0; i < attackData.ProjectileAmount; i++)
            {
                AttackInfo attackInfo = new AttackInfo();
                attackInfo = CalculateAttackInfo(attackData, attackInfo, i, j);
                ProjectileFactory.CreateProjectile(attackInfo);
            }
            yield return new WaitForSeconds(attackData.ConsecutiveAttackDelay);
        }
    }

    private AttackInfo CalculateAttackInfo(AttackData attackData, AttackInfo attackInfo, int i, int j)
    {

        Vector3 positionRandomness = Vector3.zero;
        float rotationRandomness = 0f;
        positionRandomness = new Vector3(Random.Range(attackData.RandomHorizontalFactorMin, attackData.RandomHorizontalFactorMax),
                                         Random.Range(attackData.RandomVerticalFactorMin, attackData.RandomVerticalFactorMax), 0);
        rotationRandomness = Random.Range(attackData.RandomRotationFactorMin, attackData.RandomRotationFactorMax);

        attackInfo.spawnPosition = transform.position;
        attackInfo.projectile = attackData.Projectile;

        if (attackData.AttackIsDirectionDependant)
        {
            attackInfo.spawnPositionOffset = new Vector3((attackData.UniversalSpawnPositionOffset.x + attackData.ProjectileSpawnPositionOffset[i].x + positionRandomness.x) * transform.right.x,
                                                          attackData.UniversalSpawnPositionOffset.y + attackData.ProjectileSpawnPositionOffset[i].y + positionRandomness.y, 0);
            attackInfo.speed = attackData.ProjectileSpeed * transform.right.x;
        }
        else
        {
            attackInfo.spawnPositionOffset = new Vector3((attackData.UniversalSpawnPositionOffset.x + attackData.ProjectileSpawnPositionOffset[i].x + positionRandomness.x),
                                                          attackData.UniversalSpawnPositionOffset.y + attackData.ProjectileSpawnPositionOffset[i].y + positionRandomness.y, 0);
            attackInfo.speed = attackData.ProjectileSpeed;
        }

        attackInfo.destroyDelay = attackData.ProjectileDestroyDelay;
        attackInfo.damage = attackData.Damage;

        if (attackData.AttackType == AttackTypeEnum.perimetrical)
        {
            attackInfo.rotation = attackData.ProjectileRotations[i] + rotationRandomness;
        }
        else
        {
            attackInfo.rotation = CalculateTargetedRotation() + attackData.ProjectileRotations[i] + rotationRandomness;
        }

        attackInfo.movementTypeEnum = attackData.ProjectileMovementType;
        attackInfo.functionMovementType = attackData.FunctionMovementType;
        return attackInfo;
    }

    private float CalculateTargetedRotation()
    {
        Vector3 difference;
        float projectileRotation;
        difference = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
        difference.Normalize();
        projectileRotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        return projectileRotation;
    }

    private void StopCoroutines(DamageType damageType)
    {
        StopAllCoroutines();
    }

}

public struct AttackInfo
{
    public Vector3 spawnPosition;
    public GameObject projectile;
    public Vector3 spawnPositionOffset;
    public float speed;
    public float destroyDelay;
    public int damage;
    public float rotation;
    public ProjectileMovementTypeEnum movementTypeEnum;
    public FunctionMovementTypeEnum functionMovementType;
}