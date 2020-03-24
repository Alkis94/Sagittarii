using System;
using System.Collections;
using UnityEngine;

public class PlayerProjectileImpact : MonoBehaviour
{
   
    public event Action OnCollision = delegate { };
    public Vector2 velocityOnHit = Vector2.zero;

    [SerializeField]
    private AudioClip arrowImpact;
    [SerializeField]
    private float impactDestroyDelay = 0.1f;

    private Collider2D collider2d;
    private AudioSource audioSource;
    private Rigidbody2D rigidbody2d;

    

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnCollision?.Invoke();
        audioSource.clip = arrowImpact;
        audioSource.Play();
        velocityOnHit = rigidbody2d.velocity;
        ArrowHit();
        Destroy(gameObject, impactDestroyDelay);
    }


    private void ArrowHit()
    {
        collider2d.enabled = false;
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.angularVelocity = 0;
        rigidbody2d.isKinematic = true;
        enabled = false;
    }
}
