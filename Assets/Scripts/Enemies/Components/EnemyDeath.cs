using UnityEngine;
using System;
using Factories;
using System.Collections;
using Cinemachine;
using Sirenix.OdinInspector;


[ShowOdinSerializedPropertiesInInspector]
public class EnemyDeath : SerializedMonoBehaviour
{
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
    private float energyDropRate = 0.015f;

    private bool hasBlood;
    private bool hasCriticalDeath;
    private bool shakeBeforeDeath;

    [SerializeField]
    private GameObject amputationPart;

    private bool diedFromCriticalHit = false;
    private Vector2 projectileVelocityOnHit;

    private void OnEnable()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyGotShot = GetComponent<EnemyGotShot>();
        enemyStats.EnemyDied += ProcessDeath;
    }

    private void OnDisable()
    {
        enemyStats.EnemyDied -= ProcessDeath;
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        enemyBrain = GetComponent<EnemyBrain>();

        hasBlood = enemyStats.HasBlood;
        hasCriticalDeath = enemyStats.HasCriticalDeath;
        shakeBeforeDeath = enemyStats.ShakeBeforeDeath;
        bloodSplat = Resources.Load("DeathBloodSplat") as GameObject;
    }

    public void ProcessDeath(DamageType lastDamageType)
    {
        spriteRenderer.sortingLayerName = "DeadEnemies";
        gameObject.layer = 14;
        enemyGotShot.enabled = false;
        transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        GetComponent<EnemyLoader>().ChangeEnemyStatusToDead(diedFromCriticalHit);
        projectileVelocityOnHit = enemyGotShot.ProjectileVelocityOnHit;
        rigidbody2d.velocity = Vector2.zero;

        if(lastDamageType == DamageType.critical)
        {
            diedFromCriticalHit = true;
        }

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

        DropRelic();
        bool pickupDropped = false;
        pickupDropped = PickUpFactory.Instance.DropPickup(transform.position, "HealthPickup", healthDropRate);
        if(!pickupDropped)
        {
            PickUpFactory.Instance.DropPickup(transform.position, "EnergyPickup", energyDropRate);
        }
        PickUpFactory.Instance.DropGold(transform.position, enemyStats.GoldDropChance, enemyStats.MinGoldGiven, enemyStats.MaxGoldGiven);
    }

    private void DropRelic()
    {
        for (int i = 0; i < enemyStats.Relics.Count; i++)
        {
            float randomNumber;
            randomNumber = UnityEngine.Random.Range(0f, 1f);
            if (randomNumber < enemyStats.RelicDropChance[i])
            {
                OnDeathDropRelic?.Invoke(enemyStats.Relics[i], transform.position);
                return;
            }
        }
    }

}
