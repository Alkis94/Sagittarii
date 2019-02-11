using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  EnemyBrain : MonoBehaviour
{

    protected EnemyData enemyData;
    protected EnemyGotShot enemyGotShot;
    protected EnemyGroundCollision enemyGroundCollision;
    protected AttackPatern attackPatern;
    protected SpriteRenderer spriteRenderer;
   
    protected float Speed;

    protected virtual void OnEnable()
    {
        enemyGroundCollision.OnGroundCollision += ChangeDirection;
        enemyGotShot.OnDeath += CancelInvoke;
    }

    protected virtual void OnDisable()
    {
        enemyGroundCollision.OnGroundCollision -= ChangeDirection;
        enemyGotShot.OnDeath -= CancelInvoke;
    }

    protected virtual void Awake()
    {
        enemyGotShot = GetComponentInChildren<EnemyGotShot>();
        enemyGroundCollision = GetComponent<EnemyGroundCollision>();
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
