using UnityEngine;

public class BunnyBrain : GroundEnemyBrain
{
    [SerializeField]
    private float rayOriginOffset = 0;


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
    }

    protected void FixedUpdate()
    {
        raycaster.UpdateRaycastOrigins();
        UpdateCollisionTracker();
        HandleWalkingAnimation();

        if (CheckHorizontalGround() && Time.time > cannotChangeDirectionTime)
        {
            cannotChangeDirectionTime = Time.time + 0.1f;
            ChangeHorizontalDirection();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + rayOriginOffset, 0);
            RaycastHit2D hit = Physics2D.Raycast(origin, transform.right, 10, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                if (hit.distance < 5)
                {
                    animator.SetTrigger("Attack");
                    Move(0);
                    enemyAttackHandler.Attack(enemyStats.AttackData[0]);
                }
                else
                {
                    Move(enemyStats.Speed + 6);
                }
            }
            else
            {
                Move(enemyStats.Speed);
            }
        }
    }
}
