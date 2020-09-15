using System;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class ProjectileHandler: MonoBehaviour
{

    public event Action OnCollision = delegate { };
    [HideInInspector]
    public Vector2 velocityOnHit = Vector2.zero;

    private AudioSource audioSource;
    private Rigidbody2D rigidbody2d;
    [SerializeField]
    private AudioClip impact;
    private float destroyDelay = 1;
    [SerializeField]
    private bool bulletSplits;
    [SerializeField]
    private bool isPenetrative = false;
    [ShowIf("@ bulletSplits")]
    [SerializeField] private EnemyAttackData attackData;

    private Animator animator;

    [SerializeField]
    private bool hasImpactAnimation = false;
    [SerializeField]
    private bool hasTravelAnimation = false;

    void Start()
    {
        destroyDelay = GetComponent<ProjectileDataInitializer>().DestroyDelay;
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DestroyAndSplit());

        animator = GetComponent<Animator>();

        if (hasTravelAnimation)
        {
            animator.SetTrigger("Travel");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isPenetrative)
        {
            if(other.gameObject.layer == 8)
            {
                HandleCollision();
            }
        }
        else
        {
            HandleCollision();
        }
    }

    private void HandleCollision()
    {
        OnCollision?.Invoke();
        velocityOnHit = rigidbody2d.velocity;

        float impactDestroyDelay = 0;
        if (hasImpactAnimation)
        {
            animator.SetTrigger("Impact");
            impactDestroyDelay = animator.GetCurrentAnimatorStateInfo(0).length;
        }

        if (impact != null)
        {
            audioSource.clip = impact;
            audioSource.Play();
        }

        
        StopProjectile();

        if (bulletSplits)
        {
            GetComponent<EnemyAttackHandler>().Attack(attackData);
        }

        Destroy(gameObject, impactDestroyDelay);
    }

    private void StopProjectile()
    {
        gameObject.layer = 14;
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.angularVelocity = 0;
        rigidbody2d.isKinematic = true;
        enabled = false;
    }

    IEnumerator DestroyAndSplit()
    {
        yield return new WaitForSeconds(destroyDelay);
        if(bulletSplits)
        {
            GetComponent<EnemyAttackHandler>().Attack(attackData);
        }
        Destroy(gameObject);
    }

}
