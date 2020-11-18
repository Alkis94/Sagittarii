using UnityEngine;

public class MushroomKingBrain : GroundEnemyBrain
{
    [HideInInspector]
    public StateMachine<MushroomKingBrain> stateMachine;
    [HideInInspector]
    public MushroomKingPhase1 mushroomKingPhase1;
    [HideInInspector]
    public MushroomKingPhase2 mushroomKingPhase2;
    public RuntimeAnimatorController phase2Controller;
    [SerializeField]
    private GameObject walkShroom;

    protected override void Awake()
    {
        base.Awake();
        mushroomKingPhase1 = new MushroomKingPhase1(this);
        mushroomKingPhase2 = new MushroomKingPhase2(this);
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
        collisionTracker = GetComponentInChildren<CollisionTracker>();
        raycaster = GetComponentInChildren<Raycaster>();

        stateMachine = new StateMachine<MushroomKingBrain>(this);
        stateMachine.ChangeState(mushroomKingPhase1);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    protected void CallAttack1()
    {
        enemyAttackHandler.Attack(enemyStats.AttackData[1]);
    }

    protected void CallAttack2()
    {
        enemyAttackHandler.Attack(enemyStats.AttackData[2]);
    }

    public void SpawnWalkshroom()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, -6.5f, 0);
        Instantiate(walkShroom, spawnPosition, Quaternion.identity, transform.parent);
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetTrigger("GroundCollision");
        }
    }
}
