using UnityEngine;
using System.Collections;

public class BunnyBrain : GroundEnemyBrain
{

    protected override void Awake()
    {
        base.Awake();
        enemyGroundMovement = GetComponent<EnemyGroundMovement>();
        collisionTracker = GetComponentInChildren<CollisionTracker>();
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
        CancelInvoke();
    }

    protected override void FixedUpdate()
    {

        CheckCollisions();
        //enemyGroundMovement.Move(enemyStats.Speed);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 15, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                if (hit.distance < 2)
                {
                    StartAttackAnimation();
                    enemyGroundMovement.Move(0);
                    enemyAttackHandler.Attack(enemyStats.AttackData[0]);
                }
                else
                {
                    enemyGroundMovement.Move(enemyStats.Speed + 4);
                }
            }
            else
            {
                enemyGroundMovement.Move(enemyStats.Speed);
            }
        }
    }
}
