using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBrain : EnemyBrain
{
    private Rigidbody2D rigidbody2d;
    private CollisionTracker collisionTracker;
    private Raycaster raycaster;
    private Animator animator;
    private MovementPattern movementPattern;
    private AttackPattern attackPattern;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        attackPattern = GetComponent<AttackPattern>();
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
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        InvokeRepeating("Attack", enemyData.delayBeforeFirstAttack, enemyData.attackFrequencies[0]);
        base.Start();
    }

    private void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            attackPattern.Attack();
            animator.SetTrigger("Attack");
            audioSource.Play();
        }
    }

    protected override void ChangeHorizontalDirection()
    {

    }

}
