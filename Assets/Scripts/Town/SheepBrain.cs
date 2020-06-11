using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SheepBrain : MonoBehaviour
{
    private CollisionTracker collisionTracker;
    private Rigidbody2D rigidbody2d;
    private Raycaster raycaster;
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField]
    private List<AudioClip> lampSounds;

    private float cannotChangeDirectionTime = 0;

    private void Awake()
    {
        collisionTracker = GetComponentInChildren<CollisionTracker>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        raycaster = GetComponentInChildren<Raycaster>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(MoveAround());
        StartCoroutine(BaaRandomly());
    }

    private void Update()
    {
        CheckCollisions();
    }

    IEnumerator MoveAround()
    {
        while (true)
        {
            float randomDuration = Random.Range(5, 15);
            yield return new WaitForSeconds(randomDuration);
            int randomNumber = Random.Range(100, 150);
            animator.SetTrigger("Move");
            animator.SetBool("Walking", true);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length); 
            if (randomNumber > 75)
            {
                ChangeHorizontalDirection();
            }
            for (int i = 0; i < randomNumber; i++)
            {
                Move(3);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            rigidbody2d.velocity = Vector2.zero;
            animator.SetBool("Walking", false);
        }
    }

    IEnumerator BaaRandomly()
    {
        while(true)
        {
            float randomDuration = Random.Range(15, 30);
            yield return new WaitForSeconds(randomDuration);
            int randomNumber = Random.Range(0, 3);
            audioSource.PlayOneShot(lampSounds[randomNumber]);
        }
    }

    private void CheckCollisions()
    {

        collisionTracker.collisions.Reset();
        collisionTracker.TrackHorizontalCollisions();
        collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);

        if ((collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge()) && Time.time > cannotChangeDirectionTime)
        {
            cannotChangeDirectionTime = Time.time + 0.1f;
            ChangeHorizontalDirection();
        }
    }

    public void Move(float speed)
    {
        if (rigidbody2d.velocity.y == 0)
        {
            rigidbody2d.velocity = new Vector2(transform.right.x * speed, rigidbody2d.velocity.y);
        }
        raycaster.UpdateRaycastOrigins();
    }

    public void ChangeHorizontalDirection()
    {
        transform.localRotation = transform.localRotation.y == 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }
}
