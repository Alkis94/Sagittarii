using System;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class ProjectileHandler: MonoBehaviour
{

    public event Action OnCollision = delegate { };
    [HideInInspector]
    public Vector2 velocityOnHit = Vector2.zero;
    private Animator animator;
    private AudioSource audioSource;
    private Rigidbody2D rigidbody2d;
    [SerializeField]
    private AudioClip impactSound;
    [SerializeField]
    private bool isPenetrative = false;
    [SerializeField]
    private bool ignoresCollisions = false;

    [SerializeField]
    private bool hasImpactAnimation = false;
    [SerializeField]
    private bool hasTravelAnimation = false;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        if (hasTravelAnimation)
        {
            animator.SetTrigger("Travel");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ignoresCollisions)
        {
            if (isPenetrative)
            {
                if (other.gameObject.layer == 8)
                {
                    HandleCollision();
                }
            }
            else
            {
                HandleCollision();
            }
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

        if (impactSound != null)
        {
            audioSource.clip = impactSound;
            audioSource.Play();
        }

        StopProjectile();

        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        if(particleSystem != null)
        {
            particleSystem.Stop();
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
}
