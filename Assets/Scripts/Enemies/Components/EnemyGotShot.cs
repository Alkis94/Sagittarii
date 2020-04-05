using System;
using UnityEngine;
using System.Collections;

public class EnemyGotShot : MonoBehaviour
{

    //public event Action<Transform> OnCriticalDeath = delegate { };
    public event Action<bool> OnDeath = delegate { };
    private EnemyData enemyData;
    private EnemyWasCriticalHit enemyWasCriticalHit;
    private EnemyWasHit enemyWasHit;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject amputationPart;
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyData = GetComponent<EnemyData>();
        enemyWasCriticalHit = GetComponentInChildren<EnemyWasCriticalHit>();
        enemyWasHit = GetComponentInChildren<EnemyWasHit>();
    }

    private void OnEnable()
    {
        if (enemyWasCriticalHit != null)
        {
            enemyWasCriticalHit.OnCriticalHit += CriticalHit;
        }

        enemyWasHit.OnHit += Hit;
    }

    private void OnDisable()
    {
        if (enemyWasCriticalHit != null)
        {
            enemyWasCriticalHit.OnCriticalHit -= CriticalHit;
        }

        enemyWasHit.OnHit -= Hit;
    }

    private void Hit(int damage)
    {
        if (enemyData.damageable)
        {
            StartCoroutine(FlashRed());
            enemyData.health -= damage;

            if (enemyData.health <= 0)
            {
                OnDeath?.Invoke(false);
            }

        }
    }

    private void CriticalHit(int damage,Vector2 projectileVelocityOnHit)
    {
        if (enemyData.damageable)
        {
            StartCoroutine(FlashDarkRed());
            enemyData.health -= damage * 3;

            if (enemyData.health <= 0)
            {
                OnDeath?.Invoke(true);

                if (enemyData.amputation)
                {
                    amputationPart.SetActive(true);
                    amputationPart.GetComponent<Rigidbody2D>().AddForce(projectileVelocityOnHit / 2, ForceMode2D.Impulse);
                }

            }
        }
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

    

