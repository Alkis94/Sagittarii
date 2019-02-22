 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyDeath : MonoBehaviour
{
    private EnemyGotShot enemyGotShot;

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip DeathCry;

    [SerializeField]
    private float HealthPickupDropRate = 0.05f;
    [SerializeField]
    private float DamagePickupDropRate = 0.05f;




    private void OnEnable()
    {
        enemyGotShot = GetComponentInChildren<EnemyGotShot>();
        enemyGotShot.OnDeath += Die;
    }

    private void OnDisable()
    {
        enemyGotShot.OnDeath -= Die;
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }



    private void Die()
    {
        
        ObjectFactory.Instance.CreateDeathBloodSplat(transform);
        animator.SetTrigger("Die");

        if (rigidbody2d != null)
        {
            rigidbody2d.gravityScale = 1;
        }

        spriteRenderer.sortingLayerName = "DeadEnemies";
        Destroy(gameObject, 10.0f);
        gameObject.layer = 14;
        audioSource.clip = DeathCry;
        audioSource.Play();
        transform.gameObject.tag = "DeadEnemy";
        enemyGotShot.enabled = false;

        float randomNumber;
        randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < HealthPickupDropRate)
        {
            ObjectFactory.Instance.CreatePickup(transform, ObjectFactory.Instance.HealthPickupPrefab);
        }

        randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < DamagePickupDropRate)
        {
            ObjectFactory.Instance.CreatePickup(transform, ObjectFactory.Instance.DamagePickupPrefab);
        }
    } 
}
