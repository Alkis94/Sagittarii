using UnityEngine;


public class RestTent : MonoBehaviour , IInteractable
{
    [SerializeField]
    private GameObject restMenu;
    private PlayerStats playerStats;

    public void Interact()
    {
        restMenu.SetActive(!restMenu.activeSelf);

        if (playerStats == null)
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }
    }

    public void OnYesPress()
    {
        if(playerStats.Gold >= 10)
        {
            playerStats.Gold -= 10;
            playerStats.CurrentEnergy = playerStats.MaximumEnergy;
            restMenu.SetActive(false);
        }
    }

    public void OnNoPress()
    {
        restMenu.SetActive(false);
    }

}
