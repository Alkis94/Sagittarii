using UnityEngine;
using System.Linq;
using System;
using Factories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerAttackHandler : MonoBehaviour
{

    public PlayerAttackHolder PlayerMainAttack { get; set; }
    public PlayerAttackHolder PlayerSecondaryAttack { get; set; }
    public bool HasSecondaryAttack { get; set; } = false;
    public event Action<PlayerAttackHolder, string> OnPlayerAttackChanged = delegate { };
    
    [SerializeField]
    private PlayerAttackData attackData;
    private PlayerAttackData secondaryAttackData;
    
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

    public ProjectileMovementTypeEnum ProjectileMovementTypeMain { get => projectileMovementTypeMain; private set => projectileMovementTypeMain = value; }
    public FunctionMovementTypeEnum FunctionMovementTypeMain { get => functionMovementTypeMain; private set => functionMovementTypeMain = value; }
    public AttackTypeEnum AttackTypeMain { get => attackTypeMain; private set => attackTypeMain = value; }
    public GameObject MainProjectile { get => mainProjectile; private set => mainProjectile = value; }

    private Vector3 arrowEmitterPosition;
    private PlayerStats playerStats;
    private AudioSource audioSource;
    private PlayerLoader playerLoader;

    public PlayerAttackData AttackData
    {
        get
        {
            return attackData;
        }

        set
        {
            attackData = value;
            CalculateNewPlayerAttackData(PlayerMainAttack,AttackData,true);
            OnPlayerAttackChanged?.Invoke(PlayerMainAttack, "MainAttack");
        }
    }

    public PlayerAttackData SecondaryAttackData
    {
        get
        {
            return secondaryAttackData;
        }

        set
        {
            secondaryAttackData = value;
            CalculateNewPlayerAttackData(PlayerSecondaryAttack,SecondaryAttackData);
            OnPlayerAttackChanged?.Invoke(PlayerSecondaryAttack, "SecondaryAttack");
            HasSecondaryAttack = true;
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerMainAttack = new PlayerAttackHolder();
        PlayerSecondaryAttack = new PlayerAttackHolder();
        playerLoader = GetComponent<PlayerLoader>();
    }

    private void OnEnable()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        CalculateNewPlayerAttackData(PlayerMainAttack, AttackData, true);
    }

    // ###############################################
    // #                                             #
    // #     For main and secondary attacks          #
    // #                                             #
    // ###############################################

    //Gets called from animation!
    public void CallAttackFromAnimation()
    {
        StartCoroutine(PlayerAttack(PlayerMainAttack));
        if (HasSecondaryAttack)
        {
            StartCoroutine(PlayerAttack(PlayerSecondaryAttack));
        }
        playerStats.Ammo -= 1;
    }

    private IEnumerator PlayerAttack(PlayerAttackHolder playerAttackHolder)
    {
        if (playerAttackHolder.AttackSound != null)
        {
            audioSource.PlayOneShot(playerAttackHolder.AttackSound);
        }

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

    //Calculates and returns the final info for each attack from playerAttackHolders for main and secondary attack.
    private AttackInfo PlayerCalculateAttackInfo(PlayerAttackHolder playerAttackHolder, AttackInfo attackInfo, int i, int j)
    {
        Vector3 positionRandomness = Vector3.zero;
        float rotationRandomness = 0f;
        positionRandomness = new Vector3(UnityEngine.Random.Range(playerAttackHolder.RandomHorizontalFactorMin, playerAttackHolder.RandomHorizontalFactorMax),
                                         UnityEngine.Random.Range(playerAttackHolder.RandomVerticalFactorMin, playerAttackHolder.RandomVerticalFactorMax), 0);
        rotationRandomness = UnityEngine.Random.Range(playerAttackHolder.RandomRotationFactorMin, playerAttackHolder.RandomRotationFactorMax);


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
            attackInfo.speed = playerStats.ProjectileSpeed * transform.right.x /** projectilePower*/;
        }
        else
        {
            attackInfo.spawnPositionOffset = new Vector3((playerAttackHolder.UniversalSpawnPositionOffset.x + playerAttackHolder.ProjectileSpawnPositionOffset[i].x + positionRandomness.x),
                                                          playerAttackHolder.UniversalSpawnPositionOffset.y + playerAttackHolder.ProjectileSpawnPositionOffset[i].y + positionRandomness.y, 0);
            attackInfo.speed = playerStats.ProjectileSpeed /** projectilePower*/;
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

    //Changes the AttackHolders data depending on new data picked up by the player main through items!
    private void CalculateNewPlayerAttackData(PlayerAttackHolder playerAttackHolder, PlayerAttackData attackData, bool isMainAttack = false)
    {
        playerAttackHolder.ConsecutiveAttacks = attackData.ConsecutiveAttacks > playerAttackHolder.ConsecutiveAttacks ? attackData.ConsecutiveAttacks : playerAttackHolder.ConsecutiveAttacks;
        playerAttackHolder.ConsecutiveAttackDelay = attackData.ConsecutiveAttackDelay < playerAttackHolder.ConsecutiveAttackDelay ? attackData.ConsecutiveAttackDelay : playerAttackHolder.ConsecutiveAttackDelay;
        playerAttackHolder.ConsecutiveAttackDelay = playerAttackHolder.ConsecutiveAttackDelay < 0.1f ? 0.1f : playerAttackHolder.ConsecutiveAttackDelay;
        playerAttackHolder.ProjectileRotations = attackData.ProjectileRotations.Union(attackData.ProjectileRotations).ToList();
        playerAttackHolder.ProjectileAmount = attackData.ProjectileRotations.Count;
        playerAttackHolder.UniversalSpawnPositionOffset = Vector3.zero;
        playerAttackHolder.RandomRotationFactorMin = attackData.RandomRotationFactorMin;
        playerAttackHolder.RandomRotationFactorMax = attackData.RandomRotationFactorMax;

        if (isMainAttack)
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
        for (int i = 0; i < playerAttackHolder.ProjectileAmount; i++)
        {
            playerAttackHolder.ProjectileSpawnPositionOffset.Add(Vector3.zero);
        }
    }

    // ###############################################
    // #                                             #
    // #           For special attacks               #
    // #                                             #
    // ###############################################

    //Gets called from other attack sources like special abilities and items!
    public void SpecialAttack(PlayerAttackData playerAttackData, float damageMultiplier, float speedMultiplier)
    {
        StartCoroutine(PlayerAttack(playerAttackData, damageMultiplier, speedMultiplier));
    }

    private IEnumerator PlayerAttack(PlayerAttackData playerAttackData, float damageMultiplier, float speedMultiplier)
    {
        if (playerAttackData.AttackSound != null)
        {
            audioSource.PlayOneShot(playerAttackData.AttackSound);
        }

        for (int j = 0; j < playerAttackData.ConsecutiveAttacks; j++)
        {
            for (int i = 0; i < playerAttackData.ProjectileRotations.Count; i++)
            {
                AttackInfo attackInfo = new AttackInfo();
                attackInfo = PlayerCalculateAttackInfo(playerAttackData, attackInfo, i, j, damageMultiplier, speedMultiplier);
                ProjectileFactory.CreateProjectile(attackInfo, 11, "PlayerProjectile");
            }
            yield return new WaitForSeconds(playerAttackData.ConsecutiveAttackDelay);
        }
    }

    //Calculates final attack from simple attackData.
    private AttackInfo PlayerCalculateAttackInfo(PlayerAttackData attackData, AttackInfo attackInfo, int i, int j, float damageMultiplier, float speedMultiplier)
    {
        Vector3 positionRandomness = Vector3.zero;
        float rotationRandomness = 0f;
        positionRandomness = new Vector3(UnityEngine.Random.Range(attackData.RandomHorizontalFactorMin, attackData.RandomHorizontalFactorMax),
                                         UnityEngine.Random.Range(attackData.RandomVerticalFactorMin, attackData.RandomVerticalFactorMax), 0);
        rotationRandomness = UnityEngine.Random.Range(attackData.RandomRotationFactorMin, attackData.RandomRotationFactorMax);


        if (attackData.AttackType == AttackTypeEnum.aimed)
        {
            attackInfo.spawnPosition = projectileEmitter.transform.position;
        }
        else
        {
            attackInfo.spawnPosition = transform.position;
        }

        attackInfo.projectile = attackData.Projectile;

        if (attackData.AttackIsDirectionDependant)
        {
            attackInfo.spawnPositionOffset = new Vector3((attackData.UniversalSpawnPositionOffset.x + attackData.ProjectileSpawnPositionOffset[i].x + positionRandomness.x) * transform.right.x,
                                                          attackData.UniversalSpawnPositionOffset.y + attackData.ProjectileSpawnPositionOffset[i].y + positionRandomness.y, 0);
            attackInfo.speed = playerStats.ProjectileSpeed * transform.right.x /** projectilePower*/;
        }
        else
        {
            attackInfo.spawnPositionOffset = new Vector3((attackData.UniversalSpawnPositionOffset.x + attackData.ProjectileSpawnPositionOffset[i].x + positionRandomness.x),
                                                          attackData.UniversalSpawnPositionOffset.y + attackData.ProjectileSpawnPositionOffset[i].y + positionRandomness.y, 0);
            attackInfo.speed = playerStats.ProjectileSpeed * speedMultiplier;
        }

        attackInfo.destroyDelay = 10;
        attackInfo.damage = (int) (playerStats.Damage * damageMultiplier);

        if (attackData.AttackType == AttackTypeEnum.aimed)
        {
            attackInfo.rotation = attackData.ProjectileRotations[i] + rotationRandomness + projectileEmitter.transform.rotation.eulerAngles.z;
        }
        else
        {
            attackInfo.rotation = attackData.ProjectileRotations[i] + rotationRandomness;
        }

        attackInfo.movementTypeEnum = attackData.ProjectileMovementType;
        attackInfo.functionMovementType = attackData.FunctionMovementType;
        return attackInfo;
    }
}