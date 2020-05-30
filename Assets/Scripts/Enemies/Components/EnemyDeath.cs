using UnityEngine;
using System;
using Factories;
using System.Collections;
using Cinemachine;


public class EnemyDeath : MonoBehaviour
{
    public static event Action<Vector3, string> OnDeathDropPickup = delegate { };
    public static event Action<string, Vector3> OnDeathDropRelic = delegate { };

    private EnemyStats enemyStats;
    private EnemyGotShot enemyGotShot;
    private GameObject bloodSplat;

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private EnemyBrain enemyBrain;

    [SerializeField]
    private AudioClip deathCry;
    private float healthDropRate = 0.05f;
    private float maxHealthDropRate = 0.005f;
    private float damageDropRate = 0.001f;
    private float energyDropRate = 0.02f;
    [SerializeField]
    private bool hasBlood = true;
    [SerializeField]
    private bool hasCriticalDeath = false;
    [SerializeField]
    private bool shakeBeforeDeath = false;
    [SerializeField]
    private GameObject amputationPart;

    private bool diedFromCriticalHit = false;
    private Vector2 projectileVelocityOnHit;

    private void OnEnable()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyGotShot = GetComponent<EnemyGotShot>();
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

    public void ProcessDeath(bool criticalDeath,Vector2 projectileVelocityOnHit)
    { 
        spriteRenderer.sortingLayerName = "DeadEnemies";
        gameObject.layer = 14;
        enemyGotShot.enabled = false;
        transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        diedFromCriticalHit = criticalDeath;
        this.projectileVelocityOnHit = projectileVelocityOnHit;
        rigidbody2d.velocity = Vector2.zero;

        if (enemyBrain != null)
        {
            enemyBrain.enabled = false;
        }

        //Put child objects to deadEnemies layer too because colliders are child objects.
        foreach (Transform trans in GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = 14;
        }

        if (shakeBeforeDeath)
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
        animator.SetTrigger("ShakeBeforeDeath");
        CinemachineImpulseSource impulseSource;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        impulseSource.GenerateImpulse();

        Color red = new Color(1f, 0f, 0f);
        Color white = new Color(1f, 1f, 1f);
        Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), 0);

        for (int i = 0; i < 10; i++)
        {
            Instantiate(bloodSplat, transform.position + randomVector, Quaternion.identity);
            randomVector = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), 0);
            spriteRenderer.color = red;
            yield return new WaitForSeconds(0.2f);
            Instantiate(bloodSplat, transform.position + randomVector, Quaternion.identity);
            randomVector = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), 0);
            spriteRenderer.color = white;
            yield return new WaitForSeconds(0.2f);
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
            if (enemyStats.Amputation)
            {
                amputationPart.SetActive(true);
                amputationPart.GetComponent<Rigidbody2D>().AddForce(projectileVelocityOnHit / 2, ForceMode2D.Impulse);
            }
        }
        else
        {
            animator.SetTrigger("Die");
        }

        audioSource.clip = deathCry;
        audioSource.Play();
        rigidbody2d.gravityScale = 1;
        rigidbody2d.velocity = Vector2.zero;
        transform.parent = null;

        if (enemyStats.Relic != "")
        {
            float randomNumber;
            randomNumber = UnityEngine.Random.Range(0f, 1f);
            if (randomNumber < enemyStats.RelicDropChance)
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
        OnDeathDropRelic?.Invoke(enemyStats.Relic, transform.position);
    }

    private void DropGold()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < enemyStats.GoldDropChance)
        {
            int minGoldGiven = enemyStats.MinGoldGiven / 5;
            int maxGoldGiven = enemyStats.MaxGoldGiven / 5;
            int finalGoldGiven = UnityEngine.Random.Range(minGoldGiven, maxGoldGiven + 1) * 5;

            int GoldCoins = finalGoldGiven / 20;
            int SilverCoins = (finalGoldGiven - GoldCoins * 20) / 10;
            int CooperCoins = (finalGoldGiven - (GoldCoins * 20 + SilverCoins * 10)) / 5;

            for (int i = 0; i < GoldCoins; i++)
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

}
