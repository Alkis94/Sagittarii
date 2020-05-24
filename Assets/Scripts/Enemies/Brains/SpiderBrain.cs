using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBrain : EnemyBrain
{

    private MovementPattern movementPattern;
    private AttackPattern attackPattern;

    [SerializeField]
    private GameObject web;

    protected override void Awake()
    {
        base.Awake();
        attackPattern = GetComponent<AttackPattern>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Instantiate(web, transform.position, Quaternion.identity);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        InvokeRepeating("Attack", enemyStats.DelayBeforeFirstAttack, enemyStats.AttackData[0].AttackFrequency);
        base.Start();
    }

    private void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            attackPattern.Attack(enemyStats.AttackData[0]);
            animator.SetTrigger("Attack");
            audioSource.Play();
        }
    }

    protected override void ChangeHorizontalDirection()
    {

    }

}
