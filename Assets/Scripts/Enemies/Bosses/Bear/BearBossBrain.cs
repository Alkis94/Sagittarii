using Cinemachine;
using System.Collections;
using UnityEngine;

public class BearBossBrain : GroundEnemyBrain
{
    private Transform player;
    [SerializeField]
    private GameObject teleport;
    [SerializeField]
    private AudioClip wakeUpSound;
    [SerializeField]
    private AudioClip groundStompSound;

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
        if (EnemyStats.Health > 0)
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
        if (EnemyStats.Health <= 0)
        {
            return;
        }

        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("BearBossWalking"))
        {
            Move(EnemyStats.Speed);
        }
        else
        {
            Stop();
        }
    }

    private void CheckCollisions()
    {
        UpdateCollisionTracker();

        var isColliding = CollisionTracker.collisions.left || CollisionTracker.collisions.right || CollisionTracker.CloseToGroundEdge();

        if (CollisionTracker.collisions.below && !isColliding)
        {
            Animator.SetBool(animatorAbleToWalk, true);
        }
        else
        {
            Animator.SetBool(animatorAbleToWalk, false);
        }
    }

    private IEnumerator WakeUp()
    {
        AudioSource.PlayOneShot(wakeUpSound);
        var impulseSource = GetComponent<CinemachineImpulseSource>();
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = wakeUpSound.length;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = wakeUpSound.length / 10;
        impulseSource.GenerateImpulse();
        yield return new WaitForSeconds(wakeUpSound.length);
        Animator.SetTrigger("WakeEnd");
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
                Animator.SetTrigger("StartSmash");
                AudioSource.PlayOneShot(groundStompSound);
                randomTime = Random.Range(2f, 4f);
                yield return new WaitForSeconds(randomTime);
                Animator.SetTrigger("EndSmash");
                randomTime = Random.Range(0.5f, 1f);
                previousAttackWasSmash = true;
                normalAttackCounter = 0;
            }
            else
            {
                normalAttackCounter++;
                Animator.SetTrigger("Attack");
                randomTime = Random.Range(1f, 1.5f);
                previousAttackWasSmash = false;
            }
        }
    }

    // Called from animation event
    protected void CallAttack(int attackIndex)
    {
        EnemyAttackHandler.Attack(EnemyStats.AttackData[attackIndex]);
    }
}
