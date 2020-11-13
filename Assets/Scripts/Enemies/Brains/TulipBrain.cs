using UnityEngine;
using System.Collections;

public class TulipBrain : EnemyBrain
{
    private Transform player;

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
        StartCoroutine(Attack());
    }

    private void FixedUpdate()
    {
        LookTowardsPlayer(transform, player.position);
    }

    private IEnumerator Attack ()
    {
        while(true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 40, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(enemyStats.AttackData[0].AttackFrequency);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
