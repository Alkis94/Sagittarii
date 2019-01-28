using UnityEngine;
using Factories;


public class PlayerFireProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject ArrowEmitter;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileDestroyDelay = 30f;


    private Animator animator;
    private Vector3 ArrowEmitterPosition;
    
    private delegate void VoidDelegate(float someFloat);
    private VoidDelegate FireArrow;

    private float AttackHoldAnimationLength = 0.333f;
    private float TimePassedHoldingArrow = 0f;
    private float ArrowPower;


    void Start()
    {
        animator = GetComponent<Animator>();
        //Debug.Log("Current Animation: " + animator.GetCurrentAnimatorStateInfo(0).IsName("IdleHands"));

        if (ItemHandler.ItemDropped["DeadBird"])
        {
            FireArrow = FireArrowWithBird;
        }
        else
        {
            FireArrow = FireArrowWithoutBird;
        }
    }

    void Update()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && animator.GetCurrentAnimatorStateInfo(0).IsName("IdleHands"))
        {
            TimePassedHoldingArrow = 0;
            animator.SetTrigger("PlayerAttackHold");
        }
        else if (Input.GetButton("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHold"))
        {
            TimePassedHoldingArrow += Time.deltaTime;
            ArrowPower += Time.deltaTime;
        }
        else if (Input.GetButtonUp("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackHold"))
        {
            ArrowPower = TimePassedHoldingArrow / AttackHoldAnimationLength;
            ArrowPower = ArrowPower > 1 ? 1 : ArrowPower;
            Debug.Log("Arrow Power: " + ArrowPower);
            if (ArrowPower > 0.01)
            {
                animator.SetTrigger("PlayerAttackRelease");
                FireArrow(ArrowPower);
            }
            else
            {
                animator.SetTrigger("PlayerAttackCanceled");
            }
        }
    }

    void OnEnable()
    {
        PlayerCollision.OnPlayerGotDeadBird+= PlayerGotDeadBird;
    }


    void OnDisable()
    {
        PlayerCollision.OnPlayerGotDeadBird -= PlayerGotDeadBird;
    }


    private void FireArrowWithoutBird(float arrowPower)
    {
        ProjectileFactory.CreateProjectile(ArrowEmitter.transform.position,  projectile, Vector3.zero, projectileSpeed*ArrowPower, projectileDestroyDelay, ArrowEmitter.transform.rotation);
    }

    private void FireArrowWithBird(float arrowPower)
    {
        ProjectileFactory.CreateProjectile(ArrowEmitter.transform.position, projectile, Vector3.zero, projectileSpeed * ArrowPower, projectileDestroyDelay, ArrowEmitter.transform.rotation);
        ProjectileFactory.CreateProjectile(ArrowEmitter.transform.position, projectile, Vector3.zero, projectileSpeed * ArrowPower, projectileDestroyDelay, ArrowEmitter.transform.rotation);
        ProjectileFactory.CreateProjectile(ArrowEmitter.transform.position, projectile, Vector3.zero, projectileSpeed * ArrowPower, projectileDestroyDelay, ArrowEmitter.transform.rotation);
    }

    private void PlayerGotDeadBird()
    {
        FireArrow = FireArrowWithBird;
    }


}