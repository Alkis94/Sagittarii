using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ThiefTeleport : SpecialAbility
{
    [SerializeField]
    private GameObject teleportPortal;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        Cooldown = 3f;
    }

    public override void CastSpecialAbility()
    {
        if (playerStats.CurrentEnergy > 0 && SceneManager.GetActiveScene().name != "Town" && timeTillNextCast < Time.time)
        {
            var result = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.8f, 1 << LayerMask.NameToLayer("Ground"));
            if (result == null)
            {
                
                if (specialAbilitySound != null)
                {
                    audioSource.PlayOneShot(specialAbilitySound);
                }
                StartCoroutine(Teleport());
                timeTillNextCast = Time.time + Cooldown;
                playerStats.CurrentEnergy--;
                UIManager.Instance.UpdateSpecial(Cooldown);
            }
        }
    }

    private IEnumerator Teleport()
    {
        Instantiate(teleportPortal, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(teleportPortal, transform.position, Quaternion.identity);
    }
}
