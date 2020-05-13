using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class AttackPattern : MonoBehaviour
{
    [SerializeField]
    private List<AttackData> attackData;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GetComponent<EnemyData>().EnemyDied += StopCoroutines;
    }

    private void OnDisable()
    {
        GetComponent<EnemyData>().EnemyDied -= StopCoroutines;
    }

    public void Attack(int index)
    {
        if (attackData[index].AttackSound != null)
        {
            audioSource.PlayOneShot(attackData[index].AttackSound);
        }

        StartCoroutine(StartAttacking(index));
    }

    IEnumerator StartAttacking(int index)
    {
        for (int j = 0; j < attackData[index].ConsecutiveAttacks; j++)
        {
            for (int i = 0; i < attackData[index].ProjectileAmount; i++)
            {
                AttackInfo attackInfo = new AttackInfo();
                attackInfo = CalculateAttackInfo(index, attackInfo, i, j);
                ProjectileFactory.CreateProjectile(attackInfo);
            }
            yield return new WaitForSeconds(attackData[index].ConsecutiveAttackDelay);
        }
    }

    private AttackInfo CalculateAttackInfo(int index, AttackInfo attackInfo, int i, int j)
    {

        Vector3 positionRandomness = Vector3.zero;
        float rotationRandomness = 0f;

        if (attackData[index].Randomness)
        {
            positionRandomness = new Vector3(Random.Range(attackData[index].RandomHorizontalFactorMin, attackData[index].RandomHorizontalFactorMax),
                                              Random.Range(attackData[index].RandomVerticalFactorMin, attackData[index].RandomVerticalFactorMax), 0);
            rotationRandomness = Random.Range(attackData[index].RandomRotationFactorMin, attackData[index].RandomRotationFactorMax);
        }

        attackInfo.spawnPosition = transform.position;
        attackInfo.projectile = attackData[index].Projectile;

        if (attackData[index].AttackIsDirectionDependant)
        {
            attackInfo.spawnPositionOffset = new Vector3((attackData[index].UniversalSpawnPositionOffset.x + attackData[index].ProjectileSpawnPositionOffset[i].x + positionRandomness.x) * transform.right.x,
                                                          attackData[index].UniversalSpawnPositionOffset.y + attackData[index].ProjectileSpawnPositionOffset[i].y + positionRandomness.y, 0);
            attackInfo.speed = attackData[index].ProjectileSpeed * transform.right.x;
        }
        else
        {
            attackInfo.spawnPositionOffset = new Vector3((attackData[index].UniversalSpawnPositionOffset.x + attackData[index].ProjectileSpawnPositionOffset[i].x + positionRandomness.x),
                                                          attackData[index].UniversalSpawnPositionOffset.y + attackData[index].ProjectileSpawnPositionOffset[i].y + positionRandomness.y, 0);
            attackInfo.speed = attackData[index].ProjectileSpeed;
        }

        attackInfo.destroyDelay = attackData[index].ProjectileDestroyDelay;
        attackInfo.damage = attackData[index].Damage;

        if (attackData[index].AttackType == AttackTypeEnum.perimetrical)
        {
            attackInfo.rotation = attackData[index].ProjectileRotations[i] + rotationRandomness;
        }
        else
        {
            attackInfo.rotation = CalculateTargetedRotation() + rotationRandomness;
        }

        attackInfo.movementTypeEnum = attackData[index].projectileMovementType;
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

    private void StopCoroutines()
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
}