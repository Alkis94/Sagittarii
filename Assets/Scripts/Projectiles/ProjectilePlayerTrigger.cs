using System;
using System.Collections;
using UnityEngine;

public class ProjectilePlayerTrigger : MonoBehaviour
{
    [SerializeField]
    private AudioClip arrowImpact;
    [SerializeField]
    private AudioClip arrowGroundImpact;

    private Collider2D collider2d;
    private AudioSource audioSource;

    [SerializeField]
    private float impactDestroyDelay = 30;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer enemySpriteRenderer;
    private EnemyGotShot enemyGotShot;

    public event Action OnCollision = delegate { };

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            OnCollision?.Invoke();
            collider2d.enabled = false;
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.angularVelocity = 0;
            rigidbody2d.isKinematic = true;
            enabled = false;
            
            if (other.tag == "Enemy")
            {
                transform.parent = other.transform;
                audioSource.clip = arrowImpact;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = arrowGroundImpact;
                audioSource.Play();
                Destroy(gameObject, impactDestroyDelay);
            }
    }

}
