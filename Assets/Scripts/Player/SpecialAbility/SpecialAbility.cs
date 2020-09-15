using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class SpecialAbility : MonoBehaviour
{
    private float timeTillNextCast = 0;

    protected PlayerStats playerStats;
    public float Cooldown { get; set; }
   

    // Use this for initialization
    protected virtual void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public virtual void CastSpecialAbility()
    {
        if (playerStats.CurrentEnergy > 0 && SceneManager.GetActiveScene().name != "Town" && timeTillNextCast < Time.time)
        {
            timeTillNextCast = Time.time + Cooldown;
            playerStats.CurrentEnergy--;
            UIManager.Instance.UpdateSpecial(Cooldown);
            Ability();
        }
    }

    protected abstract void Ability();
}
