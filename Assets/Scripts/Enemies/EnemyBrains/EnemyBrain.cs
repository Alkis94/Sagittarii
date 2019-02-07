using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  EnemyBrain : MonoBehaviour
{

    protected EnemyData enemyData;
    protected EnemyCollision enemyCollision;
    protected AttackPatern attackPatern;
    protected SpriteRenderer spriteRenderer;
   
    protected float Speed;

    protected virtual void Awake()
    {
        enemyCollision = GetComponent<EnemyCollision>();
        enemyData = GetComponent<EnemyData>();
        attackPatern = GetComponent<AttackPatern>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        Speed = enemyData.Speed;

        if (enemyData.ChangingDirections)
        {
            InvokeRepeating("ChangeDirection", enemyData.ChangeDirectionFrequency, enemyData.ChangeDirectionFrequency);
        }

        StartFacingRandomDirection();
    }

    protected abstract void ChangeDirection();

    protected void StartFacingRandomDirection()
    {
        float random = Random.Range(0f, 1f);
        if (random < 0.5f)
        {
            ChangeDirection();
        }
    }
}
