using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class SpecialAbility : MonoBehaviour
{
    public float Cooldown { get; set; }

    [SerializeField]
    protected AudioClip specialAbilitySound;
    protected AudioSource audioSource;
    protected PlayerStats playerStats;
    protected float timeTillNextCast = 0;

    protected virtual void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();
    }

    //public virtual void CastSpecialAbility()
    //{
    //    if (playerStats.CurrentEnergy > 0 && SceneManager.GetActiveScene().name != "Town" && timeTillNextCast < Time.time)
    //    {
    //        if(specialAbilitySound != null)
    //        {
    //            audioSource.PlayOneShot(specialAbilitySound);
    //        }

    //        timeTillNextCast = Time.time + Cooldown;
    //        playerStats.CurrentEnergy--;
    //        UIManager.Instance.UpdateSpecial(Cooldown);
    //        Ability();
    //    }
    //}

    public abstract void CastSpecialAbility();
}
