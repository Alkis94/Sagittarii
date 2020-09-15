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

    protected override void Ability()
    {
        animator.SetTrigger("Special");
        playerAttackHandler.SpecialAttack(playerAttackData, 3, 4);
    }

}
