using UnityEngine;
using TMPro;

public class DoctorShop : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject healthMenu;
    private PlayerStats playerStats;

    [SerializeField]
    private TextMeshProUGUI healCostText;
    [SerializeField]
    private TextMeshProUGUI healAmountText;

    private int missingHealthPercentage;
    private int healAmount = 0;
    private int healCost = 0;

    void Start()
    {
        healthMenu.SetActive(false);
        healCostText.text = "0";
        healAmountText.text = "0%";
    }

    public void Interact()
    {
        OpenHealthMenu();
    }

    public void OpenHealthMenu()
    {
        healthMenu.SetActive(!healthMenu.activeSelf);

        if(playerStats == null)
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }

        float currentHealthPercentage = (float)playerStats.CurrentHealth / (float)playerStats.MaximumHealth * 100;
        missingHealthPercentage = 100 - (int)(currentHealthPercentage);
    }

    public void OnMaxClick()
    {
        healAmount = missingHealthPercentage;
        healCost = missingHealthPercentage;
        if(healAmount > playerStats.Gold)
        {
            healAmount = playerStats.Gold;
            healCost = playerStats.Gold;
        }
        healAmountText.text = healAmount.ToString() + "%";
        healCostText.text = healCost.ToString();
    }

    public void OnPlusAmountClick()
    {
        if (healAmount < missingHealthPercentage)
        {
            if(healAmount < missingHealthPercentage - 10)
            {
                healAmount += 10;
                healCost += 10;
            }
            else
            {
                healAmount += missingHealthPercentage - healAmount;
                healCost += missingHealthPercentage - healCost;
            }
            
            healAmountText.text = healAmount.ToString() + "%";
            healCostText.text = healCost.ToString();
        }
    }

    public void OnMinusAmountClick()
    {
        if (0 < healAmount)
        {
            healAmount -= 10;
            healCost -= 10;

            if(healAmount < 0)
            {
                healAmount = 0;
                healCost = 0;
            }
            healAmountText.text = healAmount.ToString() + "%";
            healCostText.text = healCost.ToString();
        }
    }

    public void OnHealClick()
    {
        if (playerStats.Gold >= healCost)
        {
            playerStats.ApplyHeal(healAmount * playerStats.MaximumHealth / 100);
            playerStats.Gold -= healCost;
            ResetHealthMenu();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ResetHealthMenu();
        }
    }

    private void ResetHealthMenu()
    {
        healAmount = 0;
        healCost = 0;
        healAmountText.text =  "0%";
        healCostText.text = "0";
        healthMenu.SetActive(false);
    }

}
