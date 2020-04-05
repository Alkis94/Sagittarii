using UnityEngine;
using System;
using Factories;
using System.Collections;


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


    


    [SerializeField]
    private AudioClip deathCry;
    

    //[SerializeField]
    private float healthDropRate = 0.05f;
    //[SerializeField]
    private float maxHealthDropRate = 0.01f;
    //[SerializeField]
    private float damageDropRate = 0.01f;
    //[SerializeField]
    private float energyDropRate = 0.1f;
    [SerializeField]
    private bool hasBlood = true;
    [SerializeField]
    private bool hasCriticalDeath = false;
    [SerializeField]
    private bool hasBeforeDeath = false;

    private bool diedFromCriticalHit = false;



    private void OnEnable()
    {
        enemyData = GetComponent<EnemyData>();

        if (enemyData.Relic != null)
        {
            relicData = enemyData.Relic.GetComponent<RelicData>();
        }

        enemyGotShot = GetComponentInChildren<EnemyGotShot>();
        enemyGotShot.OnDeath += ProcessDeath;
    }

    private void OnDisable()
    {
        enemyGotShot.OnDeath -= ProcessDeath;
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        enemyBrain = GetComponent<EnemyBrain>();
   

        bloodSplat = Resources.Load("DeathBloodSplat") as GameObject;
    }

    private void ProcessDeath(bool criticalDeath)
    { 
        spriteRenderer.sortingLayerName = "DeadEnemies";
        gameObject.layer = 14;
        audioSource.clip = deathCry;
        audioSource.Play();
        enemyGotShot.enabled = false;
        transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        diedFromCriticalHit = criticalDeath;

        if (enemyBrain != null)
        {
            enemyBrain.enabled = false;
        }

        //Put child objects to deadEnemies layer too because colliders are child objects.
        foreach (Transform trans in GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = 14;
        }

        if (hasBeforeDeath)
        {
            StartCoroutine(BeforeDeath());
        }
        else
        {
            Die();
        }
    }

    IEnumerator BeforeDeath()
    {
        animator.SetTrigger("BeforeDeath");

        Color red = new Color(1f, 0f, 0f);
        Color white = new Color(1f, 1f, 1f);

        for (int i = 0; i < 8; i++)
        {
            spriteRenderer.color = red;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = white;
            yield return new WaitForSeconds(0.25f);
        }

        spriteRenderer.color = white;
        Die();
    }

    private void Die()
    {
        if (hasBlood)
        {
            Instantiate(bloodSplat, transform.position, Quaternion.identity);
        }

        if(diedFromCriticalHit && hasCriticalDeath)
        {
            animator.SetTrigger("DieCritical");
        }
        else
        {
            animator.SetTrigger("Die");
        }

        rigidbody2d.gravityScale = 1;

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

        randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < energyDropRate)
        {
            OnDeathDropPickup?.Invoke(transform.position, "EnergyPickup");
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
