using UnityEngine;
using System.Linq;
using Factories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerAttackHandler : MonoBehaviour
{
    [SerializeField]
    private AttackData attackData;
    private AttackData secondaryAttackData;
    private PlayerAttackHolder playerMainAttack;
    private PlayerAttackHolder playerSecondaryAttack;
    private bool hasSecondaryAttack = false;

    [SerializeField]
    private ProjectileMovementTypeEnum projectileMovementTypeMain;
    [SerializeField]
    private FunctionMovementTypeEnum functionMovementTypeMain;
    [SerializeField]
    private AttackTypeEnum attackTypeMain;
    [SerializeField]
    private GameObject mainProjectile;
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
            CalculateNewPlayerAttackData(playerMainAttack,AttackData,true);
            SaveAttack(playerMainAttack, "MainAttack");
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
            SaveAttack(playerSecondaryAttack, "SecondaryAttack");
            hasSecondaryAttack = true;
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
        playerStats = GetComponentInParent<PlayerStats>();

        if (ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack"))
        {
            if(ES3.KeyExists("MainAttackProjectileAmount", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack"))
            {
                LoadAttack(playerMainAttack, "MainAttack");
            }
            else
            {
                CalculateNewPlayerAttackData(playerMainAttack, AttackData, true);
            }

            if (ES3.KeyExists("SecondaryAttackProjectileAmount", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack"))
            {
                LoadAttack(playerSecondaryAttack, "SecondaryAttack");
                hasSecondaryAttack = true;
            }
        }
        else
        {
            CalculateNewPlayerAttackData(playerMainAttack, AttackData, true);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetAnimator;
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
        if (hasSecondaryAttack)
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

    private void CalculateNewPlayerAttackData(PlayerAttackHolder playerAttackHolder, AttackData attackData, bool isMainAttack = false)
    {
        
        playerAttackHolder.ConsecutiveAttacks = attackData.ConsecutiveAttacks > playerAttackHolder.ConsecutiveAttacks ? attackData.ConsecutiveAttacks : playerAttackHolder.ConsecutiveAttacks;
        playerAttackHolder.ConsecutiveAttackDelay = attackData.ConsecutiveAttackDelay < playerAttackHolder.ConsecutiveAttackDelay ? attackData.ConsecutiveAttackDelay : playerAttackHolder.ConsecutiveAttackDelay;
        playerAttackHolder.ConsecutiveAttackDelay = playerAttackHolder.ConsecutiveAttackDelay < 0.1f ? 0.1f : playerAttackHolder.ConsecutiveAttackDelay;
        playerAttackHolder.ProjectileRotations = attackData.ProjectileRotations.Union(attackData.ProjectileRotations).ToList();
        playerAttackHolder.ProjectileAmount = attackData.ProjectileRotations.Count;
        playerAttackHolder.UniversalSpawnPositionOffset = Vector3.zero;
        playerAttackHolder.RandomRotationFactorMin = attackData.RandomRotationFactorMin;
        playerAttackHolder.RandomRotationFactorMax = attackData.RandomRotationFactorMax;

        if(isMainAttack)
        {
            playerAttackHolder.Projectile = mainProjectile;
            playerAttackHolder.ProjectileMovementType = projectileMovementTypeMain;
            playerAttackHolder.FunctionMovementType = functionMovementTypeMain;
            playerAttackHolder.AttackType = attackTypeMain;

        }
        else
        {
            playerAttackHolder.Projectile = attackData.Projectile;
            playerAttackHolder.ProjectileMovementType = attackData.ProjectileMovementType;
            playerAttackHolder.FunctionMovementType = attackData.FunctionMovementType;
            playerAttackHolder.AttackType = attackData.AttackType;
        }
        

        playerAttackHolder.ProjectileSpawnPositionOffset.Clear();
        for(int i = 0; i < playerAttackHolder.ProjectileAmount; i++)
        {
            playerAttackHolder.ProjectileSpawnPositionOffset.Add(Vector3.zero);
        }
    }

    private void SaveAttack (PlayerAttackHolder playerAttackHolder, string attackType)
    {
        if(attackType != "MainAttack")
        {
            ES3.Save<GameObject>(attackType + "Projectile", playerAttackHolder.Projectile, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            ES3.Save<int>(attackType + "ProjectileMovement", (int)playerAttackHolder.ProjectileMovementType, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            ES3.Save<int>(attackType + "FunctionMovementType", (int)playerAttackHolder.FunctionMovementType, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            ES3.Save<int>(attackType + "AttackType", (int)playerAttackHolder.AttackType, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        }
        ES3.Save<int>(attackType + "ConsecutiveAttacks",playerAttackHolder.ConsecutiveAttacks , "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<float>(attackType + "ConsecutiveAttackDelay", playerAttackHolder.ConsecutiveAttackDelay , "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<List<float>>(attackType + "ProjectileRotations", playerAttackHolder.ProjectileRotations , "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<int>(attackType + "ProjectileAmount", playerAttackHolder.ProjectileAmount , "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<Vector3>(attackType + "UniversalSpawnPositionOffset", playerAttackHolder.UniversalSpawnPositionOffset , "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<float>(attackType + "RandomRotationFactorMin", playerAttackHolder.RandomRotationFactorMin , "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<float>(attackType + "RandomRotationFactorMax", playerAttackHolder.RandomRotationFactorMax, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
    }

    private void LoadAttack(PlayerAttackHolder playerAttackHolder, string attackType)
    {
        if(attackType == "MainAttack")
        {
            playerAttackHolder.Projectile = mainProjectile;
            playerAttackHolder.ProjectileMovementType = projectileMovementTypeMain;
            playerAttackHolder.FunctionMovementType = functionMovementTypeMain;
            playerAttackHolder.AttackType = attackTypeMain;
        }
        else
        {
            playerAttackHolder.Projectile = ES3.Load<GameObject>(attackType + "Projectile", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            playerAttackHolder.ProjectileMovementType = (ProjectileMovementTypeEnum)ES3.Load<int>(attackType + "ProjectileMovement", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            playerAttackHolder.FunctionMovementType = (FunctionMovementTypeEnum)ES3.Load<int>(attackType + "FunctionMovementType", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            playerAttackHolder.AttackType = (AttackTypeEnum)ES3.Load<int>(attackType + "AttackType", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        }
        playerAttackHolder.ConsecutiveAttacks = ES3.Load<int>(attackType + "ConsecutiveAttacks",  "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.ConsecutiveAttackDelay = ES3.Load<float>(attackType + "ConsecutiveAttackDelay", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.ProjectileRotations = ES3.Load<List<float>>(attackType + "ProjectileRotations", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.ProjectileAmount = ES3.Load<int>(attackType + "ProjectileAmount", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.UniversalSpawnPositionOffset = ES3.Load<Vector3>(attackType + "UniversalSpawnPositionOffset",  "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.RandomRotationFactorMin = ES3.Load<float>(attackType + "RandomRotationFactorMin", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.RandomRotationFactorMax = ES3.Load<float>(attackType + "RandomRotationFactorMax", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");

        playerAttackHolder.ProjectileSpawnPositionOffset.Clear();
        for (int i = 0; i < playerAttackHolder.ProjectileAmount; i++)
        {
            playerAttackHolder.ProjectileSpawnPositionOffset.Add(Vector3.zero);
        }
    }

}