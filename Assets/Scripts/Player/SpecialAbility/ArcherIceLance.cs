using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcherIceLance : SpecialAbility
{
    [SerializeField]
    private PlayerAttackHandler playerAttackHandler;
    [SerializeField]
    private PlayerAttackData playerAttackData;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentsInChildren<Animator>()[1];
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        Cooldown = 5f;
    }

    public override void CastSpecialAbility()
    {
        if (playerStats.CurrentEnergy > 0 && SceneManager.GetActiveScene().name != "Town" && timeTillNextCast < Time.time)
        {
            if (specialAbilitySound != null)
            {
                audioSource.PlayOneShot(specialAbilitySound);
            }
            animator.SetTrigger("Special");
            playerAttackHandler.SpecialAttack(playerAttackData, 3, 4);
            timeTillNextCast = Time.time + Cooldown;
            playerStats.CurrentEnergy--;
            UIManager.Instance.UpdateSpecial(Cooldown);
        }
    }

}
