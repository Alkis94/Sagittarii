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

    public abstract void CastSpecialAbility();
}
