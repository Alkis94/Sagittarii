using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool PlayerNotDead = true;
    public AudioSource PlayerGotHitSound;

    public delegate void VoidDelegate();
    public static event VoidDelegate OnDeath;
    public static event VoidDelegate OnPlayerGotBatWings;
    public static event VoidDelegate OnPlayerGotDeadBird;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile" && PlayerNotDead)
        {
            PlayerStats.PlayerHealth -= 10;
            UIManager.Instance.UpdateHealth(PlayerStats.PlayerHealth, PlayerStats.PlayerMaximumHealth);
            if (PlayerStats.PlayerHealth < 1)
            {
                PlayerNotDead = false;
                if(OnDeath != null) OnDeath();
            }
            else
            {
                PlayerGotHitSound.Play();
            }
        }

        if (other.tag == "HealthPickup" && PlayerStats.PlayerMaximumHealth > PlayerStats.PlayerHealth)
        {
            PlayerStats.PlayerHealth += 10;
            UIManager.Instance.UpdateHealth(PlayerStats.PlayerHealth, PlayerStats.PlayerMaximumHealth);
        }
        if (other.tag == "MaximumHealthPickup")
        {
            PlayerStats.PlayerHealth += 10;
            PlayerStats.PlayerMaximumHealth += 10;
            UIManager.Instance.UpdateHealth(PlayerStats.PlayerHealth, PlayerStats.PlayerMaximumHealth);
        }
        if (other.tag == "BatWingsPickup")
        {
            ItemHandler.PlayerHasBatWings = true;
            if (OnPlayerGotBatWings != null) OnPlayerGotBatWings();

        }
        if (other.tag == "WolfPawPickup")
        {
            ItemHandler.WolfPawMultiplier += 1;
        }
        if (other.tag == "DeadBirdPickup")
        {
            ItemHandler.ItemDropped["DeadBird"] = true;
            if (OnPlayerGotDeadBird != null) OnPlayerGotDeadBird();
        }
        if (other.tag == "ImpFlamePickup")
        {
            ItemHandler.PlayerHasImpFlame = true;
            PlayerStats.PlayerDamage += 10;
        }

    }

}
