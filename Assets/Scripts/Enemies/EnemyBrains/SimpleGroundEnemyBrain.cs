using UnityEngine;

public class SimpleGroundEnemyBrain : EnemyBrain
{

    private Animator animator;
    private MovementController movementController;
    private MovementCollisionHandler movementCollisionHandler;

    protected override void Start()
    {
        base.Start();
        movementController = GetComponent<MovementController>();
        movementCollisionHandler = GetComponent<MovementCollisionHandler>();
        animator = GetComponent<Animator>();
        InvokeRepeating("Attack", enemyData.DelayBeforeFirstAttack, enemyData.AttackFrequency);
        

        if (enemyData.ChangingDirections)
        {
            InvokeRepeating("ChangeDirection", enemyData.ChangeDirectionFrequency, enemyData.ChangeDirectionFrequency);
        }
    }


    void Update()
    {
        if (movementCollisionHandler.collisions.left || movementCollisionHandler.collisions.right)
        {
            ChangeDirection();
        }

        if (enemyData.Health > 1)
        {
            movementController.Move(enemyData.Speed, false);
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        attackPatern.Attack(enemyData);
    }

    protected void ChangeDirection()
    {
        movementController.ChangeDirection();
        transform.localRotation = transform.right.x > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }
}
