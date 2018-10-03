using UnityEngine;
using System.Collections;

public class Wolf : Enemy , IInitializable
{

    //public AudioSource WolfDeathCry;
    public AudioSource WolfAttackSound;
    private Coroutine AttackCouroutine;

    void Start ()
    {
        Health = C.WOLF_HEALTH;
        Speed = C.WOLF_SPEED;
        EnemyRB2D = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyRenderer = GetComponent<SpriteRenderer>();
        EnemyBoxCollider = GetComponent<BoxCollider2D>();
        AttackCouroutine = StartCoroutine(WolfAttack(C.WOLF_ATTACK_FREQUENCY));

    }

    public void Initialize(Transform spawnPointPosition, int horizontalDirection, int verticalDirection)
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

    private IEnumerator WolfAttack(float attackFrequency)
    {
        while (true)
        {
            yield return new WaitForSeconds(attackFrequency);
            EnemyAnimator.SetTrigger("Attack");
            EnemyRB2D.AddForce(new Vector2(7.5f * HorizontalDirection, 4), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.6f);
            WolfAttackSound.Play();
            ObjectFactory.Instance.CreateProjectile<WolfProjectile>(transform, HorizontalDirection, 0.1f,ObjectFactory.Instance.WolfProjectilePrefab);
            ObjectFactory.Instance.CreateProjectile<WolfProjectile>(transform, HorizontalDirection, 0,ObjectFactory.Instance.WolfProjectilePrefab);
            ObjectFactory.Instance.CreateProjectile<WolfProjectile>(transform, HorizontalDirection, -0.1f, ObjectFactory.Instance.WolfProjectilePrefab);
        }
    }

    protected override void EnemyDie()
    {
        StopCoroutine(AttackCouroutine);
        EnemyBoxCollider.size = new Vector3(0.4f, 0.3f, 0);
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < C.WOLF_PAW_DROP_RATE)
        {
            ObjectFactory.Instance.CreatePickup(transform,ObjectFactory.Instance.WolfPawPickupPrefab);
        }

        base.EnemyDie();
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
