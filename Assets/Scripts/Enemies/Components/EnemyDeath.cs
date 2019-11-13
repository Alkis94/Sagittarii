 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Factories;


public class EnemyDeath : MonoBehaviour
{
    public static event Action<Vector3, string> OnDeathDropPickup = delegate { };
    public static event Action<GameObject, Vector3> OnDeathDropRelic = delegate { };

    private EnemyData enemyData;
    private RelicData relicData;
    private EnemyGotShot enemyGotShot;
    private GameObject bloodSplat;

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private EnemyBrain enemyBrain;
    private AdvancedEnemyBrain advancedEnemyBrain;

    [SerializeField]
    private AudioClip deathCry;

    //[SerializeField]
    private float healthDropRate = 0.05f;
    //[SerializeField]
    private float maxHealthDropRate = 0.01f;
    //[SerializeField]
    private float damageDropRate = 0.01f;
    [SerializeField]
    private bool hasBlood = true;
    [SerializeField]
    private float enemyDestroyDelay = 10f;




    private void OnEnable()
    {
        enemyData = GetComponent<EnemyData>();

        if (enemyData.Relic != null)
        {
            relicData = enemyData.Relic.GetComponent<RelicData>();
        }

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
        enemyBrain = GetComponent<EnemyBrain>();
        advancedEnemyBrain = GetComponent<AdvancedEnemyBrain>();

        bloodSplat = Resources.Load("DeathBloodSplat") as GameObject;
    }



    private void Die()
    {
        if (hasBlood)
        {
            Instantiate(bloodSplat, transform.position, Quaternion.identity);
        }

        animator.SetTrigger("Die");
        Destroy(gameObject, enemyDestroyDelay + animator.GetCurrentAnimatorStateInfo(0).length);
        rigidbody2d.gravityScale = 1;
        spriteRenderer.sortingLayerName = "DeadEnemies";
        gameObject.layer = 14;
        audioSource.clip = deathCry;
        audioSource.Play();
        transform.gameObject.tag = "DeadEnemy";
        enemyGotShot.enabled = false;
        transform.parent = null;

        if (enemyBrain != null)
        {
            enemyBrain.enabled = false;
        }
        else
        {
            advancedEnemyBrain.enabled = false;
        }

        foreach (Transform trans in GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = 14;
        }

        if (enemyData.Relic != null)
        {
            float randomNumber;
            randomNumber = UnityEngine.Random.Range(0f, 1f);
            if (randomNumber < relicData.dropRate && !RelicFactory.PlayerHasRelic[enemyData.Relic.name])
            {
                DropRelic();
                return;
            }
        }

        DropPickup();
        DropGold();
    }

    private void DropPickup()
    {
        float randomNumber;
        randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < healthDropRate)
        {
            OnDeathDropPickup?.Invoke(transform.position, "HealthPickup");
            return;
        }

        randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < maxHealthDropRate)
        {
            OnDeathDropPickup?.Invoke(transform.position, "MaxHealthPickup");
            return;
        }

        randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < damageDropRate)
        {
            OnDeathDropPickup?.Invoke(transform.position, "DamagePickup");
            return;
        }
    }

    private void DropRelic()
    {
        OnDeathDropRelic?.Invoke(enemyData.Relic, transform.position);
    }

    private void DropGold()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 1f);
        int finalGoldGiven = enemyData.goldGiven;

        if(randomNumber < 0.01f)
        {
            finalGoldGiven = UnityEngine.Random.Range(finalGoldGiven * 5, finalGoldGiven * 10);
        }
        else if (randomNumber < 0.3)
        {
            finalGoldGiven = UnityEngine.Random.Range(finalGoldGiven * 2, finalGoldGiven * 5);
        }
        else  
        {
            finalGoldGiven = UnityEngine.Random.Range(finalGoldGiven * 1, finalGoldGiven * 2);
        }

        randomNumber = UnityEngine.Random.Range(0f, 1f);

        if (randomNumber < 0.01f)
        {
            finalGoldGiven += 10;
        }
        else if (randomNumber < 0.1)
        {
            finalGoldGiven += 5;
        }

        int GoldCoins = finalGoldGiven / 10;
        int SilverCoins = (finalGoldGiven - GoldCoins * 10) / 5;
        int CooperCoins = finalGoldGiven % 5;

        for(int i = 0; i < GoldCoins; i++)
        {
            OnDeathDropPickup?.Invoke(transform.position, "Gold");
        }

        for (int i = 0; i < SilverCoins; i++)
        {
            OnDeathDropPickup?.Invoke(transform.position, "Silver");
        }

        for (int i = 0; i < CooperCoins; i++)
        {
            OnDeathDropPickup?.Invoke(transform.position, "Cooper");
        }

    }

}
