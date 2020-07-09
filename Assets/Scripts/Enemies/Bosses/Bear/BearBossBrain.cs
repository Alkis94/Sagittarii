using UnityEngine;
using System.Collections;


public class BearBossBrain : EnemyBrain
{
    private EnemyGroundMovement enemyGroundMovement;
    private Transform player;
    [SerializeField]
    private GameObject teleport;
    [SerializeField]
    private AudioClip wakeUpSound;
    [SerializeField]
    private AudioClip groundStompSound;

    private bool ableToMove = true;
    private bool isDoingGroundAttack = false;
    private int animatorAbleToWalk;

    private Vector3 teleport1 = new Vector3(-7.75f, -4.443924f, 0);
    private Vector3 teleport2 = new Vector3(6.15f, -4.443924f, 0);
    private Vector3 teleport3 = new Vector3(20.5f, -4.443924f, 0);

    protected override void Awake()
    {
        base.Awake();
        enemyGroundMovement = GetComponent<EnemyGroundMovement>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    { 
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animatorAbleToWalk = Animator.StringToHash("AbleToWalk");
        StartCoroutine(DoRandomAction());
        audioSource.PlayOneShot(wakeUpSound);
    }

    private void Update()
    {
        if (enemyStats.Health > 0)
        {
            CheckCollisions();

            if (transform.position.x < player.position.x - 0.5f && transform.right.x < 0)
            {
                ChangeHorizontalDirection();
            }
            else if (transform.position.x > player.position.x + 0.5f && transform.right.x > 0)
            {
                ChangeHorizontalDirection();
            }
        }
    }

    private void FixedUpdate()
    {
        if (enemyStats.Health > 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("BearBossWalking"))
        {
            enemyGroundMovement.Move(enemyStats.Speed);
        }
    }


    protected override void ChangeHorizontalDirection()
    {
        enemyGroundMovement.ChangeHorizontalDirection();
    }

    private void CheckCollisions()
    {
        collisionTracker.collisions.Reset();
        collisionTracker.TrackHorizontalCollisions();
        collisionTracker.TrackVerticalCollisions(rigidbody2d.velocity.y);
        
        if ((collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge()))
        {
            ableToMove = false;
        }
        else
        {
            ableToMove = true;
        }

        HandleWalkingAnimation();
    }

    private void HandleWalkingAnimation()
    {
        if (collisionTracker.collisions.below && ableToMove)
        {

            animator.SetBool(animatorAbleToWalk, true);
        }
        else
        {

            animator.SetBool(animatorAbleToWalk, false);
        }
    }

    IEnumerator DoRandomAction()
    {
        float randomTime = 3;
        float nextTeleportTime = Random.Range(7.5f, 10) + Time.time;
        float randomNumber;
        while (true)
        {
            yield return new WaitForSeconds(randomTime);

            if (Time.time > nextTeleportTime)
            {
                nextTeleportTime = Random.Range(4f, 6f) + Time.time;
                randomTime = Random.Range(0.25f, 0.35f);
                BearTeleport();
            }
            else
            {
                randomNumber = Random.Range(0, 1f);
                if (randomNumber < 0.3f)
                {
                    StartGroundAttack();
                    randomTime = Random.Range(1f, 2f);
                }
                else
                {
                    StartAttackAnimation();
                    randomTime = Random.Range(1, 2f);
                }
            }
        }
    }

    protected void StartGroundAttack()
    {
        animator.SetTrigger("AttackGround");
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.Impulse(0, Random.Range(25,35));
        isDoingGroundAttack = true;
    }

    //gets called from GroundStart animation;
    protected void CallImpulse()
    {
        rigidbody2d.velocity = Vector3.zero;
        rigidbody2d.Impulse(0, -40);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" && isDoingGroundAttack /*animator.GetCurrentAnimatorStateInfo(0).IsName("BearBossGroundStart")*/)
        {
            isDoingGroundAttack = false;
            audioSource.PlayOneShot(groundStompSound);
            animator.SetTrigger("GroundCollision");
            enemyAttackHandler.Attack(enemyStats.AttackData[1]);
        }
    }

    private void BearTeleport()
    {
        Instantiate(teleport, transform.position, Quaternion.identity);

        float distance1 = Vector3.Distance(teleport1, player.position);
        float distance2 = Vector3.Distance(teleport2, player.position);
        float distance3 = Vector3.Distance(teleport3, player.position);

        if(distance1 < distance2)
        {
            if(distance1 < distance3)
            {
                transform.position = teleport1;
            }
            else
            {
                transform.position = teleport3;
            }
        }
        else if (distance2 < distance3) transform.position = teleport2;
        else transform.position = teleport3;
    }

}
