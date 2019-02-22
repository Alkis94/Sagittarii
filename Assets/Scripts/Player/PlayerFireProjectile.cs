using UnityEngine;
using Factories;
using System;


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
    private Action<float> FireArrow = delegate {};
    public static event Action OnPlayerFiredProjectile = delegate { };

    private float AttackHoldAnimationLength = 0.333f;
    private float TimePassedHoldingArrow = 0f;
    private float ArrowPower;


    void Start()
    {
        animator = GetComponent<Animator>();

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
        if (PlayerStats.Ammo > 0)
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
                if (ArrowPower > 0.01)
                {
                    animator.SetTrigger("PlayerAttackRelease");
                    FireArrow(ArrowPower);
                    PlayerStats.Ammo -= 1;
                    OnPlayerFiredProjectile.Invoke();
                }
                else
                {
                    animator.SetTrigger("PlayerAttackCanceled");
                }
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