using UnityEngine;

public class Bat : Enemy
{

    private float TimePassed;

    public GameObject BatProjectile;
    public AudioSource BatScreech;


    // Use this for initialization
    void Start ()
    {
        EnemyRB2D = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyRenderer = GetComponent<SpriteRenderer>();
        EnemyBoxCollider = GetComponent<BoxCollider2D>();
        Speed = C.BAT_SPEED;
        Health = C.BAT_HEALTH;
        TimePassed = 0;
        InvokeRepeating("BatChangeDirection", 3,3);
        InvokeRepeating("BatAttack", C.BAT_ATTACK_FREQUENCY, C.BAT_ATTACK_FREQUENCY);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Health > 1)
        {
            EnemyMove();
        }
	}

    protected void BatChangeDirection()
    {
        HorizontalDirection *= -1;
    }

    protected override void EnemyMove()
    {
        TimePassed += Time.deltaTime;
        VerticalDirection = Mathf.Sin(TimePassed);
        base.EnemyMove();
    }

    private void BatAttack()
    {
       Instantiate(BatProjectile, gameObject.transform.position,Quaternion.identity);
    }

    protected override void EnemyDie()
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < C.BAT_WINGS_DROP_RATE && ItemHandler.BatWingsNotDropped)
        {
            ObjectFactory.Instance.CreatePickup(transform,ObjectFactory.Instance.BatWingsPickupPrefab);
            ItemHandler.BatWingsNotDropped = false;
        }
        base.EnemyDie();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Arrow")
        {
            Health -=  PlayerStats.PlayerDamage;
            if(Health < 1)
            {
                EnemyDie();
            }
        }
    }


}
