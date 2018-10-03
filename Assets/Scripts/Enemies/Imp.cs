using UnityEngine;
using System.Collections;

public class Imp : Enemy , IInitializable
{

    // Use this for initialization
    void Start ()
    {
        Health = C.IMP_HEALTH;
        Speed = C.IMP_SPEED;
        EnemyRB2D = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyRenderer = GetComponent<SpriteRenderer>();
        EnemyBoxCollider = GetComponent<BoxCollider2D>();
        InvokeRepeating("ImpAttack", C.IMP_ATTACK_FREQUENCY, C.IMP_ATTACK_FREQUENCY);
    }

    public void Initialize (Transform spawnPointPosition, int horizontalDirection, int verticalDirection)
    {
        HorizontalDirection = horizontalDirection;
        transform.position = spawnPointPosition.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Health > 1)
        {
            EnemyMove();
            EnemyChangeDirection();
        }

    }


    private void ImpAttack()
    {
        EnemyAnimator.SetTrigger("ImpAttack");
        ObjectFactory.Instance.CreateProjectile<ImpProjectile>(transform, HorizontalDirection,0,ObjectFactory.Instance.ImpProjectilePrefab);
    }

    protected override void EnemyDie()
    {
        EnemyBoxCollider.size = new Vector3(0.4f, 0.3f, 0);
        base.EnemyDie();
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < C.IMP_FLAME_DROP_RATE)
        {
            ObjectFactory.Instance.CreatePickup(transform,ObjectFactory.Instance.ImpFlamePickupPrefab);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow")
        {
            Health -= 10 * ItemHandler.Instance.ImpFlameMultiplier;
            if (Health < 1)
            {
                EnemyDie();
            }
        }
    }
}
