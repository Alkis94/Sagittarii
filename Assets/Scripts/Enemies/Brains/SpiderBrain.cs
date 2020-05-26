using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBrain : EnemyBrain
{

    private MovementPattern movementPattern;
    private AttackPattern attackPattern;
    [SerializeField]
    private GameObject web;
    private bool wasNotLoaded = true;

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
    
    public override void LoadEnemyBrain(Vector3 originalPosition,bool dead)
    {
        wasNotLoaded = false;
        GetComponent<SpiderCrawlDown>().Load(originalPosition, dead);
        Instantiate(web, originalPosition, Quaternion.identity);
    }

    protected override void Start()
    {
        if(wasNotLoaded)
        {
            Instantiate(web, transform.position, Quaternion.identity);
        }

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
