using UnityEngine;
using System.Linq;
using Factories;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerAttackHandler : MonoBehaviour
{
    [SerializeField]
    private AttackData attackData;
    private AttackData secondaryAttackData;
    private PlayerAttackHolder playerMainAttack;
    private PlayerAttackHolder playerSecondaryAttack;

    [SerializeField]
    private GameObject projectileEmitter;
 
    private Animator animator;
    private Vector3 arrowEmitterPosition;
    private PlayerStats playerStats;
    private AudioSource audioSource;
    
    private float attackHoldAnimationLength = 0.333f;
    private float attackHoldAnimationSpeed;
    private float timePassedHoldingAttack = 0f;
    private float projectilePower;

    public AttackData AttackData
    {
        get
        {
            return attackData;
        }

        set
        {
            attackData = value;
            CalculateNewPlayerAttackData(playerMainAttack,AttackData);
        }
    }

    public AttackData SecondaryAttackData
    {
        get
        {
            return secondaryAttackData;
        }

        set
        {
            secondaryAttackData = value;
            CalculateNewPlayerAttackData(playerSecondaryAttack,SecondaryAttackData);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerMainAttack = new PlayerAttackHolder();
        playerSecondaryAttack = new PlayerAttackHolder();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetAnimator;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetAnimator;
    }

    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        CalculateNewPlayerAttackData(playerMainAttack,AttackData);
    }

    void Update()
    {
        if (playerStats.Ammo > 0)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && animator.GetCurrentAnimatorStateInfo(0).IsName("IdleHands"))
            {
                projectilePower = 0;
                timePassedHoldingAttack = 0;
                animator.SetTrigger("AttackHold");
                audioSource.Play();
            }
            else if (Input.GetButton("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("AttackHold"))
            {
                timePassedHoldingAttack += Time.deltaTime;
                attackHoldAnimationSpeed = animator.GetCurrentAnimatorStateInfo(0).speedMultiplier;

            }
            else if (Input.GetButtonUp("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsName("AttackHold"))
            {
                projectilePower = timePassedHoldingAttack * attackHoldAnimationSpeed / attackHoldAnimationLength;
                projectilePower = projectilePower > 1 ? 1 : projectilePower;
                if (projectilePower > 0.3)
                {
                    animator.SetTrigger("AttackRelease");
                    
                }
                else
                {
                    animator.SetTrigger("AttackCanceled");
                }
            }
        }
    }

    public void CallAttackFromAnimation()
    {
        PlayerAttack(playerMainAttack);
        if (secondaryAttackData != null)
        {
            PlayerAttack(playerSecondaryAttack);
        }
        playerStats.Ammo -= 1;
    }

    public void PlayerAttack(PlayerAttackHolder playerAttackHolder)
    {
        if (playerAttackHolder.AttackSound != null)
        {
            audioSource.PlayOneShot(playerAttackHolder.AttackSound);
        }

        StartCoroutine(PlayerStartAttacking(playerAttackHolder));
    }

    IEnumerator PlayerStartAttacking(PlayerAttackHolder playerAttackHolder)
    {
        for (int j = 0; j < playerAttackHolder.ConsecutiveAttacks; j++)
        {
            for (int i = 0; i < playerAttackHolder.ProjectileAmount; i++)
            {
                AttackInfo attackInfo = new AttackInfo();
                attackInfo = PlayerCalculateAttackInfo(playerAttackHolder, attackInfo, i, j);
                ProjectileFactory.CreateProjectile(attackInfo, 11, "PlayerProjectile");
            }
            yield return new WaitForSeconds(playerAttackHolder.ConsecutiveAttackDelay);
        }
    }

    private AttackInfo PlayerCalculateAttackInfo(PlayerAttackHolder playerAttackHolder, AttackInfo attackInfo, int i, int j)
    {

        Vector3 positionRandomness = Vector3.zero;
        float rotationRandomness = 0f;
        positionRandomness = new Vector3(Random.Range(playerAttackHolder.RandomHorizontalFactorMin, playerAttackHolder.RandomHorizontalFactorMax),
                                         Random.Range(playerAttackHolder.RandomVerticalFactorMin, playerAttackHolder.RandomVerticalFactorMax), 0);
        rotationRandomness = Random.Range(playerAttackHolder.RandomRotationFactorMin, playerAttackHolder.RandomRotationFactorMax);


        if (playerAttackHolder.AttackType == AttackTypeEnum.aimed)
        {
            attackInfo.spawnPosition = projectileEmitter.transform.position;
        }
        else
        {
            attackInfo.spawnPosition = transform.position;
        }

        attackInfo.projectile = playerAttackHolder.Projectile;

        if (playerAttackHolder.AttackIsDirectionDependant)
        {
            attackInfo.spawnPositionOffset = new Vector3((playerAttackHolder.UniversalSpawnPositionOffset.x + playerAttackHolder.ProjectileSpawnPositionOffset[i].x + positionRandomness.x) * transform.right.x,
                                                          playerAttackHolder.UniversalSpawnPositionOffset.y + playerAttackHolder.ProjectileSpawnPositionOffset[i].y + positionRandomness.y, 0);
            attackInfo.speed = playerStats.ProjectileSpeed * transform.right.x * projectilePower;
        }
        else
        {
            attackInfo.spawnPositionOffset = new Vector3((playerAttackHolder.UniversalSpawnPositionOffset.x + playerAttackHolder.ProjectileSpawnPositionOffset[i].x + positionRandomness.x),
                                                          playerAttackHolder.UniversalSpawnPositionOffset.y + playerAttackHolder.ProjectileSpawnPositionOffset[i].y + positionRandomness.y, 0);
            attackInfo.speed = playerStats.ProjectileSpeed * projectilePower;
        }

        attackInfo.destroyDelay = playerAttackHolder.ProjectileDestroyDelay;
        attackInfo.damage = playerStats.Damage;

        if (playerAttackHolder.AttackType == AttackTypeEnum.aimed)
        {
            attackInfo.rotation = playerAttackHolder.ProjectileRotations[i] + rotationRandomness + projectileEmitter.transform.rotation.eulerAngles.z;
        }
        else
        {
            attackInfo.rotation = playerAttackHolder.ProjectileRotations[i] + rotationRandomness;
        }

        attackInfo.movementTypeEnum = playerAttackHolder.ProjectileMovementType;
        attackInfo.functionMovementType = playerAttackHolder.FunctionMovementType;
        return attackInfo;
    }

    private void ResetAnimator(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "Town")
        {
            animator.SetTrigger("ResetHands");
        }
    }

    private void CalculateNewPlayerAttackData(PlayerAttackHolder playerAttackHolder, AttackData attackData)
    {
        playerAttackHolder.ProjectileMovementType = attackData.ProjectileMovementType;
        playerAttackHolder.FunctionMovementType = attackData.FunctionMovementType;
        playerAttackHolder.AttackType = attackData.AttackType;
        playerAttackHolder.ConsecutiveAttacks = attackData.ConsecutiveAttacks;
        playerAttackHolder.ConsecutiveAttackDelay = attackData.ConsecutiveAttackDelay;
        playerAttackHolder.ProjectileRotations = attackData.ProjectileRotations.Union(attackData.ProjectileRotations).ToList();
        playerAttackHolder.ProjectileAmount = attackData.ProjectileRotations.Count;
        playerAttackHolder.UniversalSpawnPositionOffset = Vector3.zero;
        playerAttackHolder.RandomRotationFactorMin = attackData.RandomRotationFactorMin;
        playerAttackHolder.RandomRotationFactorMax = attackData.RandomRotationFactorMax;
        playerAttackHolder.Projectile = attackData.Projectile;

        playerAttackHolder.ProjectileSpawnPositionOffset.Clear();
        for(int i = 0; i < playerAttackHolder.ProjectileAmount; i++)
        {
            playerAttackHolder.ProjectileSpawnPositionOffset.Add(Vector3.zero);
        }
    }

}