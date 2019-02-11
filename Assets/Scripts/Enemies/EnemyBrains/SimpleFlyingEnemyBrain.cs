using UnityEngine;

public class SimpleFlyingEnemyBrain : EnemyBrain
{

    private MovementPattern movementPattern;
    private float HorizontalDirection = 1;


    protected override void Awake()
    {
        base.Awake();
        movementPattern = GetComponent<MovementPattern>();

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
        InvokeRepeating("Attack", enemyData.DelayBeforeFirstAttack, enemyData.AttackFrequency);
        
    }


    void Update()
    {
        if (enemyData.Health > 1)
        {
            movementPattern.Move(Speed);
        }
    }

    private void Attack()
    {
        attackPatern.Attack(enemyData);
    }

    

    protected override void ChangeDirection()
    {
        HorizontalDirection *= -1;
        if (HorizontalDirection == -1)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (HorizontalDirection == 1)
        {

            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
