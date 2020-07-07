using UnityEngine;
using System.Collections;

public class TulipBrain : EnemyBrain
{
    private Transform player;
    private AttackPattern attackPattern;
    private int horizontalDirection = 1;

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
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Attack());
    }

    private void FixedUpdate()
    {
        if(player.position.x > transform.position.x)
        {
            horizontalDirection = 1;
        }
        else
        {
            horizontalDirection = -1;
        }

        ChangeHorizontalDirection();
    }

    private IEnumerator Attack ()
    {
        while(true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 30, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                StartAttackAnimation();
                yield return new WaitForSeconds(enemyStats.AttackData[0].AttackFrequency);
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    protected override void ChangeHorizontalDirection()
    {
        if (horizontalDirection == -1)
        {
            transform.localRotation = Quaternion.Euler(0, 180, -transform.localEulerAngles.z);
        }
        else if (horizontalDirection == 1)
        {
            transform.localRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z);
        }
    }
}
