using System;
using System.Collections;
using UnityEngine;

public class PlayerProjectileImpact : MonoBehaviour
{
    [SerializeField]
    private AudioClip arrowImpact;
    [SerializeField]
    private AudioClip arrowGroundImpact;

    private Collider2D collider2d;
    private AudioSource audioSource;
    private Rigidbody2D rigidbody2d;

    [SerializeField]
    private float impactDestroyDelay = 30;
    private int enemyID = 0;

    public bool criticalHit = false;
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
        criticalHit = false;

        if (other.tag == "Enemy" && enemyID != other.transform.parent.GetInstanceID())
        {

            enemyID = other.transform.parent.GetInstanceID();

            //CircleCollider are reservered for spots that are critical hits when shot.
            if (other is CircleCollider2D)
            {
                criticalHit = true;
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x / 2, rigidbody2d.velocity.y / 2);
            }

            else
            {
                transform.parent = other.transform;
                audioSource.clip = arrowImpact;
                audioSource.Play();
                ArrowStuck();
            }
            
        }
        else if (other.tag != "Enemy")
        {
            audioSource.clip = arrowGroundImpact;
            audioSource.Play();
            Destroy(gameObject, impactDestroyDelay);
            ArrowStuck();
        }
    }


    private void ArrowStuck()
    {
        collider2d.enabled = false;
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.angularVelocity = 0;
        rigidbody2d.isKinematic = true;
        enabled = false;
    }
}
