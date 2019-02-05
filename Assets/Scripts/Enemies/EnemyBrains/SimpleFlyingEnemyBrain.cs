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

    private void OnEnable()
    {
        enemyCollision.OnGroundCollision += ChangeDirection;
        enemyCollision.OnDeath += CancelInvoke;
        enemyCollision.OnDeath += CancelInvoke;
    }

    private void OnDisable()
    {
        enemyCollision.OnGroundCollision -= ChangeDirection;
        enemyCollision.OnDeath -= CancelInvoke;
        enemyCollision.OnDeath -= CancelInvoke;
    }

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("Attack", enemyData.DelayBeforeFirstAttack, enemyData.AttackFrequency);
        StartFacingRandomDirection();
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

    private void StartFacingRandomDirection()
    {
        float random = Random.Range(0f, 1f);
        if (random < 0.5f)
        {
            //ChangeDirection();
        }
    }

    protected void ChangeDirection()
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
