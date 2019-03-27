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

    private void OnDestroy()
    {
        if (enemyGotShot != null)
        {
            enemyGotShot.OnDeath -= StartFollowingRenderer;
        }
            
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetType() != typeof(BoxCollider2D))
        {
            OnCollision?.Invoke();
            collider2d.enabled = false;
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.angularVelocity = 0;
            rigidbody2d.isKinematic = true;
            enabled = false;
            
            if (other.tag == "Enemy")
            {
                transform.parent = other.transform.parent;
                audioSource.clip = arrowImpact;
                audioSource.Play();
                enemySpriteRenderer = other.GetComponentInParent<SpriteRenderer>();
                enemyGotShot = other.GetComponent<EnemyGotShot>();
                enemyGotShot.OnDeath += StartFollowingRenderer;
            }
            else
            {
                audioSource.clip = arrowGroundImpact;
                audioSource.Play();
                Destroy(gameObject, impactDestroyDelay);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
    }

    private void StartFollowingRenderer()
    {
        StartCoroutine(FollowRenderer());
    }

    IEnumerator FollowRenderer()
    {
        float signX;
        float signY;
        while(true)
        {
            if((enemySpriteRenderer.bounds.ClosestPoint(transform.position).x - transform.position.x) != 0)
            {
                signX =  Mathf.Sign(enemySpriteRenderer.bounds.ClosestPoint(transform.position).x - transform.position.x);
                transform.position = enemySpriteRenderer.bounds.ClosestPoint(transform.position) + new Vector3(0.25f * signX, 0, 0);
            }


            if ((enemySpriteRenderer.bounds.ClosestPoint(transform.position).y - transform.position.y) != 0)
            {
                signY =  Mathf.Sign(enemySpriteRenderer.bounds.ClosestPoint(transform.position).y - transform.position.y);
                transform.position = enemySpriteRenderer.bounds.ClosestPoint(transform.position) + new Vector3(0, 0.25f * signY, 0);
            }

            yield return null;
        }
    }

}
