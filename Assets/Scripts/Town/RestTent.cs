using UnityEngine;
using TMPro;

public class RestTent : MonoBehaviour , IInteractable
{
    [SerializeField]
    private GameObject restMenu;
    private PlayerStats playerStats;
    [SerializeField]
    private TextMeshProUGUI costText;
    private int restCost = 11;

    public void Interact()
    {
        restMenu.SetActive(!restMenu.activeSelf);

        if (playerStats == null)
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            restCost = playerStats.RestCost;
            costText.text = "Pay for rest " + restCost + " ?";
        }
    }

    public void OnYesPress()
    {
        if(playerStats.Gold >= restCost)
        {
            playerStats.Gold -= restCost;
            playerStats.CurrentEnergy = playerStats.MaximumEnergy;
            restMenu.SetActive(false);
        }
    }

    public void OnNoPress()
    {
        restMenu.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            restMenu.SetActive(false);
        }
    }

}
