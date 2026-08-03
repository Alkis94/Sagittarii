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

        if (ES3.FileExists(ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats))
        {
            LoadPlayer();
        }
        else
        {
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
        if (ES3.FileExists(ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack))
        {
            if (ES3.KeyExists("MainAttackProjectileAmount", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack))
            {
                LoadAttack(playerAttackHandler.PlayerMainAttack, "MainAttack");
            }
            if (ES3.KeyExists("SecondaryAttackProjectileAmount", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack))
            {
                LoadAttack(playerAttackHandler.PlayerSecondaryAttack, "SecondaryAttack");
                playerAttackHandler.HasSecondaryAttack = true;
            }
        }
    }

    private void SavePlayer()
    {
        if (playerStats == null)
        {
            return;
        }

        ES3.Save("Class", playerStats.CharacterClass, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("MaximumHealth", playerStats.MaximumHealth, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("CurrentHealth", playerStats.CurrentHealth, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("HealthMultiplier", playerStats.HealthMultiplier, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("MaximumEnergy", playerStats.MaximumEnergy, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("CurrentEnergy", playerStats.CurrentEnergy, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Gold", playerStats.Gold, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Ammo", playerStats.Ammo, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Damage", playerStats.Damage, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("DamageMultiplier", playerStats.DamageMultiplier, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("DamageTakenMultiplier", playerStats.DamageTakenMultiplier, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Armor", playerStats.Armor, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Speed", playerStats.Speed, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("ProjectileSpeed", playerStats.ProjectileSpeed, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Luck", playerStats.Luck, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Lifesteal", playerStats.Lifesteal, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("LifestealChance", playerStats.LifestealChance, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("Energysteal", playerStats.Energysteal, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("EnergystealChance", playerStats.EnergystealChance, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("TownTax", playerStats.TownTax, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        ES3.Save("RestCost", playerStats.RestCost, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
    }

    private void LoadPlayer()
    {
        playerStats.MaximumHealth = ES3.Load<int>("MaximumHealth", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.CurrentHealth = ES3.Load<int>("CurrentHealth", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.HealthMultiplier = ES3.Load<float>("HealthMultiplier", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.MaximumEnergy = ES3.Load<int>("MaximumEnergy", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.CurrentEnergy = ES3.Load<int>("CurrentEnergy", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.GoldWithoutGainedGold = ES3.Load<int>("Gold", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Ammo = ES3.Load<int>("Ammo", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Damage = ES3.Load<int>("Damage", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.DamageMultiplier = ES3.Load<float>("DamageMultiplier", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.DamageTakenMultiplier = ES3.Load<float>("DamageTakenMultiplier", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Armor = ES3.Load<int>("Armor", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Speed = ES3.Load<float>("Speed", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.ProjectileSpeed = ES3.Load<float>("ProjectileSpeed", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Luck = ES3.Load<float>("Luck", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Lifesteal = ES3.Load<int>("Lifesteal", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.LifestealChance = ES3.Load<float>("LifestealChance", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.Energysteal = ES3.Load<int>("Energysteal", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.EnergystealChance = ES3.Load<float>("EnergystealChance", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.TownTax = ES3.Load<int>("TownTax", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        playerStats.RestCost = ES3.Load<int>("RestCost", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
    }

    public void SaveAttack(PlayerAttackHolder playerAttackHolder, string attackType)
    {
        if (attackType != "MainAttack")
        {
            ES3.Save(attackType + "Projectile", playerAttackHolder.Projectile, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            ES3.Save(attackType + "ProjectileMovement", (int)playerAttackHolder.ProjectileMovementType, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            ES3.Save(attackType + "FunctionMovementType", (int)playerAttackHolder.FunctionMovementType, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            ES3.Save(attackType + "AttackType", (int)playerAttackHolder.AttackType, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        }
        ES3.Save(attackType + "ConsecutiveAttacks", playerAttackHolder.ConsecutiveAttacks, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "ConsecutiveAttackDelay", playerAttackHolder.ConsecutiveAttackDelay, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "ProjectileRotations", playerAttackHolder.ProjectileRotations, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "ProjectileAmount", playerAttackHolder.ProjectileAmount, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "UniversalSpawnPositionOffset", playerAttackHolder.UniversalSpawnPositionOffset, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "RandomRotationFactorMin", playerAttackHolder.RandomRotationFactorMin, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        ES3.Save(attackType + "RandomRotationFactorMax", playerAttackHolder.RandomRotationFactorMax, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
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
            playerAttackHolder.Projectile = ES3.Load<GameObject>(attackType + "Projectile", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            playerAttackHolder.ProjectileMovementType = (ProjectileMovementTypeEnum)ES3.Load<int>(attackType + "ProjectileMovement", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            playerAttackHolder.FunctionMovementType = (FunctionMovementTypeEnum)ES3.Load<int>(attackType + "FunctionMovementType", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
            playerAttackHolder.AttackType = (AttackTypeEnum)ES3.Load<int>(attackType + "AttackType", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        }
        playerAttackHolder.ConsecutiveAttacks = ES3.Load<int>(attackType + "ConsecutiveAttacks", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.ConsecutiveAttackDelay = ES3.Load<float>(attackType + "ConsecutiveAttackDelay", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.ProjectileRotations = ES3.Load<List<float>>(attackType + "ProjectileRotations", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.ProjectileAmount = ES3.Load<int>(attackType + "ProjectileAmount", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.UniversalSpawnPositionOffset = ES3.Load<Vector3>(attackType + "UniversalSpawnPositionOffset", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.RandomRotationFactorMin = ES3.Load<float>(attackType + "RandomRotationFactorMin", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);
        playerAttackHolder.RandomRotationFactorMax = ES3.Load<float>(attackType + "RandomRotationFactorMax", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerAttack);

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
        if (ES3.DirectoryExists(ProfileManager.Instance.GetProfileRunPath()))
        {
            SavePlayer();
        }
    }
}
