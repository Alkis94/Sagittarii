using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

public class AttackPattern : MonoBehaviour
{
    [SerializeField]
    protected List<AttackData> attackData;
    protected AudioSource audioSource;

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public virtual void Attack(int index)
    {
        if (attackData[index].ProjectileSpawnPositionOffset.Count == 0)
        {
            for (int i = 0; i < attackData[index].ProjectileAmount; i++)
            {
                attackData[index].ProjectileSpawnPositionOffset.Add(Vector3.zero);
            }
        }

        if (attackData[index].AttackSound != null)
        {
            audioSource.PlayOneShot(attackData[index].AttackSound);
        }

        switch(attackData[index].AttackType)
        {
            case AttackTypeEnum.perimetrical:
                StartCoroutine(PerimetricalAttack(index));
                break;
            case AttackTypeEnum.targeted:
                StartCoroutine(TargetedAttack(index));
                break;
            default:
                Debug.Log("Attack Type Not Found!");
                break;
        }
    }

    IEnumerator PerimetricalAttack(int index)
    {
        List<Vector3> positionRandomness = new List<Vector3>();
        List<float> rotationnRandomness = new List<float>();
        AddRandomness(index, ref positionRandomness, ref rotationnRandomness);



        if (attackData[index].AttackIsDirectionDependant)
        {
            for (int j = 0; j < attackData[index].ConsecutiveAttacks; j++)
            {
                for (int i = 0; i < attackData[index].ProjectileAmount; i++)
                {
                    ProjectileFactory.CreateProjectile(transform.position, attackData[index].Projectile,
                    new Vector3((attackData[index].UniversalSpawnPositionOffset.x + attackData[index].ProjectileSpawnPositionOffset[i].x + positionRandomness[i].x) * transform.right.x,
                    attackData[index].UniversalSpawnPositionOffset.y + attackData[index].ProjectileSpawnPositionOffset[i].y + positionRandomness[i].y, 0),
                    attackData[index].ProjectileSpeed * transform.right.x,
                    attackData[index].ProjectileDestroyDelay, attackData[index].Damage, attackData[index].ProjectileRotations[i] + rotationnRandomness[i]);
                }
                yield return new WaitForSeconds(attackData[index].ConsecutiveAttackDelay);
            }
        }
        else
        {
            for (int j = 0; j < attackData[index].ConsecutiveAttacks; j++)
            {
                for (int i = 0; i < attackData[index].ProjectileAmount; i++)
                {
                    ProjectileFactory.CreateProjectile(transform.position, attackData[index].Projectile, attackData[index].UniversalSpawnPositionOffset + attackData[index].ProjectileSpawnPositionOffset[i] + positionRandomness[i],
                    attackData[index].ProjectileSpeed, attackData[index].ProjectileDestroyDelay, attackData[index].Damage, attackData[index].ProjectileRotations[i] + rotationnRandomness[i]);
                }
                yield return new WaitForSeconds(attackData[index].ConsecutiveAttackDelay);
            }
        }
    }

    IEnumerator TargetedAttack(int index)
    {
        List<Vector3> positionRandomness = new List<Vector3>();
        List<float> rotationnRandomness = new List<float>();
        AddRandomness(index, ref positionRandomness, ref rotationnRandomness);

        Vector3 difference;
        float projectileRotation;
        difference = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
        difference.Normalize();
        projectileRotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        for (int j = 0; j < attackData[index].ConsecutiveAttacks; j++)
        {
            for (int i = 0; i < attackData[index].ProjectileAmount; i++)
            {
                ProjectileFactory.CreateProjectile(transform.position, attackData[index].Projectile, attackData[index].UniversalSpawnPositionOffset + attackData[index].ProjectileSpawnPositionOffset[i] + positionRandomness[i],
                attackData[index].ProjectileSpeed, attackData[index].ProjectileDestroyDelay, attackData[index].Damage, attackData[index].ProjectileRotations[i] + projectileRotation + rotationnRandomness[i]);
            }
            yield return new WaitForSeconds(attackData[index].ConsecutiveAttackDelay);
        }
    }

    private void AddRandomness(int index, ref List<Vector3> positionRandomness, ref List<float> rotationRandomness)
    {
        if (attackData[index].Randomness)
        {
            for (int i = 0; i < attackData[index].ProjectileAmount; i++)
            {
                positionRandomness.Add(new Vector3(Random.Range(attackData[index].RandomHorizontalFactorMin, attackData[index].RandomHorizontalFactorMax),
                                                  Random.Range(attackData[index].RandomVerticalFactorMin, attackData[index].RandomVerticalFactorMax), 0));
                rotationRandomness.Add(Random.Range(attackData[index].RandomRotationFactorMin, attackData[index].RandomRotationFactorMax));
            }
        }
        else
        {
            for (int i = 0; i < attackData[index].ProjectileAmount; i++)
            {
                positionRandomness.Add(Vector3.zero);
                rotationRandomness.Add(0);
            }
        }
    }

}
