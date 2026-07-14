using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerLoader : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerAttackHandler playerAttackHandler;
    
    private void OnEnable()
    {
        playerStats = GetComponent<PlayerStats>();
        playerAttackHandler = GetComponentInChildren<PlayerAttackHandler>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        playerAttackHandler.OnPlayerAttackChanged += SaveAttack;

        if (ES3.FileExists(SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats))
        {
            LoadPlayer();
        }
        else
        {
            ES3.Save("Class", (int)playerStats.CharacterClass, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
            SavePlayer();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        playerAttackHandler.OnPlayerAttackChanged -= SaveAttack;
    }

    private void Start()
    {
        if (ES3.FileExists(SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack))
        {
            if (ES3.KeyExists("MainAttackProjectileAmount", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack))
            {
                LoadAttack(playerAttackHandler.PlayerMainAttack, "MainAttack");
            }
            if (ES3.KeyExists("SecondaryAttackProjectileAmount", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack))
            {
                LoadAttack(playerAttackHandler.PlayerSecondaryAttack, "SecondaryAttack");
                playerAttackHandler.HasSecondaryAttack = true;
            }
        }
    }

    private void SavePlayer()
    {
        ES3.Save("MaximumHealth", playerStats.MaximumHealth, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("CurrentHealth", playerStats.CurrentHealth, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("HealthMultiplier", playerStats.HealthMultiplier, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("MaximumEnergy", playerStats.MaximumEnergy, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("CurrentEnergy", playerStats.CurrentEnergy, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Gold", playerStats.Gold, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Ammo", playerStats.Ammo, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Damage", playerStats.Damage, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("DamageMultiplier", playerStats.DamageMultiplier, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("DamageTakenMultiplier", playerStats.DamageTakenMultiplier, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Armor", playerStats.Armor, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Speed", playerStats.Speed, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("ProjectileSpeed", playerStats.ProjectileSpeed, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Luck", playerStats.Luck, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Lifesteal", playerStats.Lifesteal, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("LifestealChance", playerStats.LifestealChance, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Energysteal", playerStats.Energysteal, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("EnergystealChance", playerStats.EnergystealChance, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("TownTax", playerStats.TownTax, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("RestCost", playerStats.RestCost, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
    }

    private void LoadPlayer()
    {
        playerStats.MaximumHealth = ES3.Load<int>("MaximumHealth", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.CurrentHealth = ES3.Load<int>("CurrentHealth", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.HealthMultiplier = ES3.Load<float>("HealthMultiplier", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.MaximumEnergy = ES3.Load<int>("MaximumEnergy", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.CurrentEnergy = ES3.Load<int>("CurrentEnergy", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.GoldWithoutGainedGold = ES3.Load<int>("Gold", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Ammo = ES3.Load<int>("Ammo", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Damage = ES3.Load<int>("Damage", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.DamageMultiplier = ES3.Load<float>("DamageMultiplier", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.DamageTakenMultiplier = ES3.Load<float>("DamageTakenMultiplier", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Armor = ES3.Load<int>("Armor", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Speed = ES3.Load<float>("Speed", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.ProjectileSpeed = ES3.Load<float>("ProjectileSpeed", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Luck = ES3.Load<float>("Luck", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Lifesteal = ES3.Load<int>("Lifesteal", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.LifestealChance = ES3.Load<float>("LifestealChance", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Energysteal = ES3.Load<int>("Energysteal", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.EnergystealChance = ES3.Load<float>("EnergystealChance", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.TownTax = ES3.Load<int>("TownTax", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.RestCost = ES3.Load<int>("RestCost", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
    }

    public void SaveAttack(PlayerAttackHolder playerAttackHolder, string attackType)
    {
        if (attackType != "MainAttack")
        {
            ES3.Save(attackType + "Projectile", playerAttackHolder.Projectile, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            ES3.Save(attackType + "ProjectileMovement", (int)playerAttackHolder.ProjectileMovementType, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            ES3.Save(attackType + "FunctionMovementType", (int)playerAttackHolder.FunctionMovementType, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            ES3.Save(attackType + "AttackType", (int)playerAttackHolder.AttackType, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        }
        ES3.Save(attackType + "ConsecutiveAttacks", playerAttackHolder.ConsecutiveAttacks, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "ConsecutiveAttackDelay", playerAttackHolder.ConsecutiveAttackDelay, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "ProjectileRotations", playerAttackHolder.ProjectileRotations, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "ProjectileAmount", playerAttackHolder.ProjectileAmount, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "UniversalSpawnPositionOffset", playerAttackHolder.UniversalSpawnPositionOffset, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "RandomRotationFactorMin", playerAttackHolder.RandomRotationFactorMin, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "RandomRotationFactorMax", playerAttackHolder.RandomRotationFactorMax, SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
    }

    public void LoadAttack(PlayerAttackHolder playerAttackHolder, string attackType)
    {
        if (attackType == "MainAttack")
        {
            playerAttackHolder.Projectile = playerAttackHandler.MainProjectile;
            playerAttackHolder.ProjectileMovementType = playerAttackHandler.ProjectileMovementTypeMain;
            playerAttackHolder.FunctionMovementType = playerAttackHandler.FunctionMovementTypeMain;
            playerAttackHolder.AttackType = playerAttackHandler.AttackTypeMain;
        }
        else
        {
            playerAttackHolder.Projectile = ES3.Load<GameObject>(attackType + "Projectile", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            playerAttackHolder.ProjectileMovementType = (ProjectileMovementTypeEnum)ES3.Load<int>(attackType + "ProjectileMovement", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            playerAttackHolder.FunctionMovementType = (FunctionMovementTypeEnum)ES3.Load<int>(attackType + "FunctionMovementType", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            playerAttackHolder.AttackType = (AttackTypeEnum)ES3.Load<int>(attackType + "AttackType", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        }
        playerAttackHolder.ConsecutiveAttacks = ES3.Load<int>(attackType + "ConsecutiveAttacks", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.ConsecutiveAttackDelay = ES3.Load<float>(attackType + "ConsecutiveAttackDelay", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.ProjectileRotations = ES3.Load<List<float>>(attackType + "ProjectileRotations", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.ProjectileAmount = ES3.Load<int>(attackType + "ProjectileAmount", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.UniversalSpawnPositionOffset = ES3.Load<Vector3>(attackType + "UniversalSpawnPositionOffset", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.RandomRotationFactorMin = ES3.Load<float>(attackType + "RandomRotationFactorMin", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.RandomRotationFactorMax = ES3.Load<float>(attackType + "RandomRotationFactorMax", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);

        playerAttackHolder.ProjectileSpawnPositionOffset.Clear();
        for (int i = 0; i < playerAttackHolder.ProjectileAmount; i++)
        {
            playerAttackHolder.ProjectileSpawnPositionOffset.Add(Vector3.zero);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // we need this check because the player was saving after exhaustion death with 0 health on scene change!
        if(playerStats.CurrentHealth > 0 && enabled)
        {
            SavePlayer();
        }
    }

    private void OnDestroy()
    {
        if (ES3.DirectoryExists(SaveManager.Instance.GetProfileRunPath()))
        {
            SavePlayer();
        }
    }
}
