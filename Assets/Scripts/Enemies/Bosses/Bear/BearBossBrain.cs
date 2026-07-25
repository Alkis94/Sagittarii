using UnityEngine;
using System.Collections;

public class BearBossBrain : GroundEnemyBrain
{
    private Transform player;
    [SerializeField]
    private GameObject teleport;
    [SerializeField]
    private AudioClip wakeUpSound;
    [SerializeField]
    private AudioClip groundStompSound;

    private bool ableToMove = true;
    private int animatorAbleToWalk;

    protected override void Awake()
    {
        base.Awake();
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
        StartCoroutine(WakeUp());
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
            Move(enemyStats.Speed);
        }
    }

    private void CheckCollisions()
    {
        UpdateCollisionTracker();

        if (collisionTracker.collisions.left || collisionTracker.collisions.right || collisionTracker.CloseToGroundEdge())
        {
            ableToMove = false;
        }
        else
        {
            ableToMove = true;
        }

        HandleWalkingAnimation();
    }

    protected override void HandleWalkingAnimation()
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

    private IEnumerator WakeUp()
    {
        audioSource.PlayOneShot(wakeUpSound);
        yield return new WaitForSeconds(3f);
        animator.SetTrigger("WakeEnd");
        StartCoroutine(DoRandomAction());
    }

    private IEnumerator DoRandomAction()
    {
        var randomTime = 0.2f;
        var previousAttackWasSmash = true;
        var normalAttackCounter = 0;

        while (true)
        {
            yield return new WaitForSeconds(randomTime);

            if ((Random.value < 0.3f || normalAttackCounter >= 3) && !previousAttackWasSmash)
            {
                animator.SetTrigger("StartSmash");
                audioSource.PlayOneShot(groundStompSound);
                randomTime = Random.Range(2f, 4f);
                yield return new WaitForSeconds(randomTime);
                animator.SetTrigger("EndSmash");
                randomTime = Random.Range(0.5f, 1f);
                previousAttackWasSmash = true;
                normalAttackCounter = 0;
            }
            else
            {
                normalAttackCounter++;
                animator.SetTrigger("Attack");
                randomTime = Random.Range(1f, 1.5f);
                previousAttackWasSmash = false;
            }
        }
    }

    protected void CallAttack(int attackIndex)
    {
        enemyAttackHandler.Attack(enemyStats.AttackData[attackIndex]);
    }
}
