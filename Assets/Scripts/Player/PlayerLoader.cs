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

        if (ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/PlayerStats"))
        {
            LoadPlayer();
        }
        else
        {
            ES3.Save<int>("Class", (int)playerStats.CharacterClass, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
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
        if (ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack"))
        {
            if (ES3.KeyExists("MainAttackProjectileAmount", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack"))
            {
                LoadAttack(playerAttackHandler.PlayerMainAttack, "MainAttack");
            }
            if (ES3.KeyExists("SecondaryAttackProjectileAmount", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack"))
            {
                LoadAttack(playerAttackHandler.PlayerSecondaryAttack, "SecondaryAttack");
                playerAttackHandler.HasSecondaryAttack = true;
            }
        }
    }


    private void SavePlayer()
    {
        ES3.Save<int>("MaximumHealth", playerStats.MaximumHealth, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("CurrentHealth", playerStats.CurrentHealth, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<float>("HealthMultiplier", playerStats.HealthMultiplier, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("MaximumEnergy", playerStats.MaximumEnergy, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("CurrentEnergy", playerStats.CurrentEnergy, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("Gold", playerStats.Gold, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("Ammo", playerStats.Ammo, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("Damage", playerStats.Damage, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<float>("DamageMultiplier", playerStats.DamageMultiplier, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<float>("DamageTakenMultiplier", playerStats.DamageTakenMultiplier, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("Armor", playerStats.Armor, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<float>("Speed", playerStats.Speed, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<float>("ProjectileSpeed", playerStats.ProjectileSpeed, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<float>("Luck", playerStats.Luck, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("Lifesteal", playerStats.Lifesteal, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<float>("LifestealChance", playerStats.LifestealChance, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("Energysteal", playerStats.Energysteal, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<float>("EnergystealChance", playerStats.EnergystealChance, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("TimeLimit", playerStats.TimeLimit, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("TownTax", playerStats.TownTax, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        ES3.Save<int>("RestCost", playerStats.RestCost, "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
    }

    private void LoadPlayer()
    {
        playerStats.MaximumHealth = ES3.Load<int>("MaximumHealth", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.CurrentHealth = ES3.Load<int>("CurrentHealth", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.HealthMultiplier = ES3.Load<float>("HealthMultiplier", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.MaximumEnergy = ES3.Load<int>("MaximumEnergy", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.CurrentEnergy = ES3.Load<int>("CurrentEnergy", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.Gold = ES3.Load<int>("Gold", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.Ammo = ES3.Load<int>("Ammo", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.Damage = ES3.Load<int>("Damage", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.DamageMultiplier = ES3.Load<float>("DamageMultiplier", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.DamageTakenMultiplier = ES3.Load<float>("DamageTakenMultiplier", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.Armor = ES3.Load<int>("Armor", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.Speed = ES3.Load<float>("Speed", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.ProjectileSpeed = ES3.Load<float>("ProjectileSpeed", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.Luck = ES3.Load<float>("Luck", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.Lifesteal = ES3.Load<int>("Lifesteal", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.LifestealChance = ES3.Load<float>("LifestealChance", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.Energysteal = ES3.Load<int>("Energysteal", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.EnergystealChance = ES3.Load<float>("EnergystealChance", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.TimeLimit = ES3.Load<int>("TimeLimit", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.TownTax = ES3.Load<int>("TownTax", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
        playerStats.RestCost = ES3.Load<int>("RestCost", "Saves/Profile" + SaveProfile.SaveID + "/PlayerStats");
    }

    public void SaveAttack(PlayerAttackHolder playerAttackHolder, string attackType)
    {
        if (attackType != "MainAttack")
        {
            ES3.Save<GameObject>(attackType + "Projectile", playerAttackHolder.Projectile, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            ES3.Save<int>(attackType + "ProjectileMovement", (int)playerAttackHolder.ProjectileMovementType, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            ES3.Save<int>(attackType + "FunctionMovementType", (int)playerAttackHolder.FunctionMovementType, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            ES3.Save<int>(attackType + "AttackType", (int)playerAttackHolder.AttackType, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        }
        ES3.Save<int>(attackType + "ConsecutiveAttacks", playerAttackHolder.ConsecutiveAttacks, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<float>(attackType + "ConsecutiveAttackDelay", playerAttackHolder.ConsecutiveAttackDelay, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<List<float>>(attackType + "ProjectileRotations", playerAttackHolder.ProjectileRotations, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<int>(attackType + "ProjectileAmount", playerAttackHolder.ProjectileAmount, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<Vector3>(attackType + "UniversalSpawnPositionOffset", playerAttackHolder.UniversalSpawnPositionOffset, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<float>(attackType + "RandomRotationFactorMin", playerAttackHolder.RandomRotationFactorMin, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        ES3.Save<float>(attackType + "RandomRotationFactorMax", playerAttackHolder.RandomRotationFactorMax, "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
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
            playerAttackHolder.Projectile = ES3.Load<GameObject>(attackType + "Projectile", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            playerAttackHolder.ProjectileMovementType = (ProjectileMovementTypeEnum)ES3.Load<int>(attackType + "ProjectileMovement", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            playerAttackHolder.FunctionMovementType = (FunctionMovementTypeEnum)ES3.Load<int>(attackType + "FunctionMovementType", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
            playerAttackHolder.AttackType = (AttackTypeEnum)ES3.Load<int>(attackType + "AttackType", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        }
        playerAttackHolder.ConsecutiveAttacks = ES3.Load<int>(attackType + "ConsecutiveAttacks", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.ConsecutiveAttackDelay = ES3.Load<float>(attackType + "ConsecutiveAttackDelay", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.ProjectileRotations = ES3.Load<List<float>>(attackType + "ProjectileRotations", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.ProjectileAmount = ES3.Load<int>(attackType + "ProjectileAmount", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.UniversalSpawnPositionOffset = ES3.Load<Vector3>(attackType + "UniversalSpawnPositionOffset", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.RandomRotationFactorMin = ES3.Load<float>(attackType + "RandomRotationFactorMin", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");
        playerAttackHolder.RandomRotationFactorMax = ES3.Load<float>(attackType + "RandomRotationFactorMax", "Saves/Profile" + SaveProfile.SaveID + "/PlayerAttack");

        playerAttackHolder.ProjectileSpawnPositionOffset.Clear();
        for (int i = 0; i < playerAttackHolder.ProjectileAmount; i++)
        {
            playerAttackHolder.ProjectileSpawnPositionOffset.Add(Vector3.zero);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //we need this check because the player was saving after exhaustion death with 0 health on scene change!
        if(playerStats.CurrentHealth > 0)
        {
            SavePlayer();
        }
    }

    private void OnDestroy()
    {
        if (ES3.DirectoryExists("Saves/Profile" + SaveProfile.SaveID))
        {
            SavePlayer();
        }
    }
}
