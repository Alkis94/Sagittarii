using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopOrb : MonoBehaviour
{
    [SerializeField]
    private GameObject orbMenu;
    private PlayerStats playerStats;

    [SerializeField]
    private TextMeshProUGUI healCostText;
    [SerializeField]
    private TextMeshProUGUI healAmountText;

    private int missingHealth;
    private int healAmount = 0;
    private int healCost = 0;

    void Start()
    {
        orbMenu.SetActive(false);
        healCostText.text = "0";
        healAmountText.text = "0";
    }


    public void OnOrbClick()
    {
        orbMenu.SetActive(!orbMenu.activeSelf);

        if(playerStats == null)
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }

        missingHealth = playerStats.MaximumHealth - playerStats.CurrentHealth;
    }

    public void OnMaxClick()
    {
        healAmount = missingHealth;
        healCost = missingHealth;
        healAmountText.text = healAmount.ToString();
        healCostText.text = healCost.ToString();
    }

    public void OnPlusAmountClick()
    {
        if (healAmount < missingHealth)
        {
            healAmount += 10;
            healCost += 10;
            healAmountText.text = healAmount.ToString();
            healCostText.text = healCost.ToString();
        }
    }

    public void OnMinusAmountClick()
    {
        if (0 < healAmount)
        {
            healAmount -= 10;
            healCost -= 10;
            healAmountText.text = healAmount.ToString();
            healCostText.text = healCost.ToString();
        }
    }

    public void OnHealClick()
    {
        if (playerStats.Gold >= healCost)
        {
            playerStats.CurrentHealth += healAmount;
            playerStats.Gold -= healCost;
            orbMenu.SetActive(false);
        }
    }

}
