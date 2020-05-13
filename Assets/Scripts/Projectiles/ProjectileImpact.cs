using System;
using UnityEngine;
using System.Collections;

public class ProjectileImpact : MonoBehaviour
{

    public event Action OnCollision = delegate { };
    [HideInInspector]
    public Vector2 velocityOnHit = Vector2.zero;

    [SerializeField]
    private AudioClip impact;
    [SerializeField]
    private float impactDestroyDelay = 0;

    private AudioSource audioSource;
    private Rigidbody2D rigidbody2d;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
