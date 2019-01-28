 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyCollision))]
public class EnemyDeath : MonoBehaviour
{
    private EnemyCollision enemyCollision;

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    public AudioSource DeathCry;


    private void OnEnable()
    {
        enemyCollision = GetComponent<EnemyCollision>();
        enemyCollision.OnDeath += Die;
    }

    private void OnDisable()
    {
        enemyCollision.OnDeath -= Die;
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    private void Die()
    {
        float randomNumber;
        ObjectFactory.Instance.CreateDeathBloodSplat(transform);
        animator.SetTrigger("Die");
        rigidbody2d.gravityScale = 1;
        spriteRenderer.sortingLayerName = "DeadEnemies";
        Destroy(gameObject, 10.0f);
        UIManager.Instance.UpdateScore();
        gameObject.layer = 14;
        DeathCry.Play();
        transform.gameObject.tag = "DeadEnemy";

        randomNumber = Random.Range(0f, 1f);
        if (randomNumber < C.HEALTH_PICKUP_DROP_RATE)
        {
            ObjectFactory.Instance.CreatePickup(transform, ObjectFactory.Instance.HealthPickupPrefab);
        }
    }



}
