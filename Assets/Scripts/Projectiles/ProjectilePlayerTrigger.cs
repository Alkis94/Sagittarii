using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayerTrigger : MonoBehaviour
{

    public AudioClip ArrowImpact;
    public AudioClip ArrowGroundImpact;

    private Collider2D collider2d;
    private AudioSource audioSource;

    [SerializeField]
    private float ImpactDestroyDelay;
    private Rigidbody2D rigidbody2d;

    public event Action OnCollision = delegate { };

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Background")
        {
            if (OnCollision != null) OnCollision();
            collider2d.enabled = false;
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.angularVelocity = 0;
            rigidbody2d.isKinematic = true;
            enabled = false;
            Destroy(gameObject, ImpactDestroyDelay);
            if (other.tag == "Enemy")
            {
                transform.parent = other.transform;
                audioSource.clip = ArrowImpact;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = ArrowGroundImpact;
                audioSource.Play();
            }
        }
    }

}
