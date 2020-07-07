using System;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class ProjectileImpact : MonoBehaviour
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
    private float impactDestroyDelay = 0;
    [SerializeField]
    private bool bulletSplits;
    [ShowIf("@ bulletSplits")]
    [SerializeField] private EnemyAttackData attackData;

    void Start()
    {
        destroyDelay = GetComponent<ProjectileDataInitializer>().DestroyDelay;
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DestroyAndSplit());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnCollision?.Invoke();

        if(impact != null)
        {
            audioSource.clip = impact;
            audioSource.Play();
        }
        
        velocityOnHit = rigidbody2d.velocity;
        StopProjectile();

        if (bulletSplits)
        {
            GetComponent<AttackPattern>().Attack(attackData);
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
            GetComponent<AttackPattern>().Attack(attackData);
        }
        Destroy(gameObject);
    }

}
