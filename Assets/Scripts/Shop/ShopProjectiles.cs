using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopProjectiles : MonoBehaviour
{
    [SerializeField]
    private GameObject projectileMenu;
    private PlayerStats playerStats;

    [SerializeField]
    private TextMeshProUGUI projectileCostText;
    [SerializeField]
    private TextMeshProUGUI projectileAmountText;

    private int projectileCost = 0;
    private int projectileAmount = 0;

    void Start()
    {

        projectileMenu.SetActive(false);
        projectileCostText.text = "0";
        projectileAmountText.text = "0";
    }

    public void OnProjectilesClick()
    {
        projectileMenu.SetActive(!projectileMenu.activeSelf);

        if (playerStats == null)
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }
    }
    
    public void OnMinusClick()
    {
        if(projectileAmount > 0)
        {
            projectileAmount -= 50;
            projectileCost -= 50;

            if(projectileAmount < 0)
            {
                projectileAmount = 0;
                projectileCost = 0;
            }

            projectileAmountText.text = projectileAmount.ToString();
            projectileCostText.text = projectileCost.ToString();
        }
    }

    public void OnPlusClick()
    {
        projectileAmount += 50;
        projectileCost += 50;
        projectileAmountText.text = projectileAmount.ToString();
        projectileCostText.text = projectileCost.ToString();
    }

    public void OnMaxClick()
    {
        projectileAmount = playerStats.Gold;
        projectileCost = playerStats.Gold;
        projectileAmountText.text = projectileAmount.ToString();
        projectileCostText.text = projectileCost.ToString();
    }

    public void OnBuyClick()
    {
        if(playerStats.Gold >= projectileCost)
        {
            playerStats.Ammo += projectileAmount;
            playerStats.Gold -= projectileCost;
            projectileMenu.SetActive(false);
        }
    }


}
