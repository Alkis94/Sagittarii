using System;
using UnityEngine;
using System.Collections;

public class EnemyGotShot : MonoBehaviour
{

    public event Action<bool,Vector2> EnemyDiedAndHow = delegate { };
   
    private EnemyData enemyData;
    private EnemyWasCriticalHit enemyWasCriticalHit;
    private EnemyWasHit enemyWasHit;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip gotHitSound;
    private bool LastHitCritical = false;
    private Vector2 projectileVelocityOnHit;
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyData = GetComponent<EnemyData>();
        enemyWasCriticalHit = GetComponentInChildren<EnemyWasCriticalHit>();
        enemyWasHit = GetComponentInChildren<EnemyWasHit>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (enemyWasCriticalHit != null)
        {
            enemyWasCriticalHit.OnCriticalHit += CriticalHit;
        }

        enemyWasHit.OnHit += Hit;
        enemyData.EnemyDied += EnemyDiedFrom;
    }

    private void OnDisable()
    {
        if (enemyWasCriticalHit != null)
        {
            enemyWasCriticalHit.OnCriticalHit -= CriticalHit;
        }

        enemyWasHit.OnHit -= Hit;
        enemyData.EnemyDied -= EnemyDiedFrom;
    }

    private void Hit(int damage)
    {
        if (enemyData.damageable)
        {
            LastHitCritical = false;
            ProcessHit();
            StartCoroutine(FlashRed());
            enemyData.Health -= damage;
        }
    }

    private void CriticalHit(int damage,Vector2 projectileVelocityOnHit)
    {
        if (enemyData.damageable)
        {
            LastHitCritical = true;
            ProcessHit();
            StartCoroutine(FlashDarkRed());
            this.projectileVelocityOnHit = projectileVelocityOnHit;
            enemyData.Health -= damage * 3;           
        }
    }

    private void ProcessHit()
    {
        if(gotHitSound != null)
        {
            audioSource.PlayOneShot(gotHitSound);
        }
    }

    private void EnemyDiedFrom()
    {
        EnemyDiedAndHow?.Invoke(LastHitCritical,projectileVelocityOnHit);
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(1f, 0.5f, 0.25f, 1f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator FlashDarkRed()
    {
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1f, 1f, 1f);
    }
}

    

