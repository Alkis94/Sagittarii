using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyData))]
[RequireComponent(typeof(EnemyCollision))]
public class EnemyMovement : MonoBehaviour
{

    private EnemyData enemyData;
    private EnemyCollision enemyCollision;
    private MovementPattern movementPattern;

    private SpriteRenderer spriteRenderer;
   
    public float HorizontalDirection = -1;
    private float Speed;

    private void OnEnable()
    {
        enemyData = GetComponent<EnemyData>();
        movementPattern = GetComponent<MovementPattern>();
        enemyCollision = GetComponent<EnemyCollision>();
        enemyCollision.OnGroundCollision += ChangeDirection;
        enemyCollision.OnDeath += CancelInvoke;
    }

    private void OnDisable()
    {
        enemyCollision.OnGroundCollision -= ChangeDirection;
        enemyCollision.OnDeath -= CancelInvoke;
    }

    void Start()
    {
        Speed = enemyData.Speed;
        spriteRenderer = GetComponent<SpriteRenderer>();

        float random = Random.Range(0f, 1f);

        if (random < 0.5f)
        {
            ChangeDirection();
        }

        if (enemyData.ChangingDirections)
        {
            InvokeRepeating("ChangeDirection", enemyData.ChangeDirectionFrequency, enemyData.ChangeDirectionFrequency);
        }
    }

    void Update()
    {
        if (enemyData.Health > 1)
        {
            movementPattern.Move(HorizontalDirection, Speed);
        }
    }

    private void ChangeDirection()
    {
        HorizontalDirection *= -1;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
