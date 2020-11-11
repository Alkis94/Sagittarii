using UnityEngine;
using System.Collections;

public class WalkshroomBrain : GroundEnemyBrain
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            //Vector3 origin = new Vector3(transform.position.x, transform.position.y - 1, 0);
            bool result = Physics2D.OverlapCircle(transform.position, 2.5f, 1 << LayerMask.NameToLayer("Player"));
            //RaycastHit2D hit = Physics2D.Raycast(origin, transform.right, 2, 1 << LayerMask.NameToLayer("Player"));
            if (result)
            {
                StartAttackAnimation();
                enemyGroundMovement.Move(0);
                enemyAttackHandler.Attack(enemyStats.AttackData[0]);
            }
            else
            {
                enemyGroundMovement.Move(enemyStats.Speed);
            }
        }
    }
}
