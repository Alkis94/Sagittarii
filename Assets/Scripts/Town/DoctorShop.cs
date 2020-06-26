using UnityEngine;
using TMPro;

public class DoctorShop : MonoBehaviour
{
    [SerializeField]
    private GameObject healthMenu;
    private PlayerStats playerStats;

    [SerializeField]
    private TextMeshProUGUI healCostText;
    [SerializeField]
    private TextMeshProUGUI healAmountText;
    private BoxCollider2D boxCollider2D;


    private int missingHealth;
    private int healAmount = 0;
    private int healCost = 0;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        healthMenu.SetActive(false);
        healCostText.text = "0";
        healAmountText.text = "0";
    }


    public void OpenHealthMenu()
    {
        healthMenu.SetActive(!healthMenu.activeSelf);

        if(playerStats == null)
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }

        missingHealth = playerStats.MaximumHealth - playerStats.CurrentHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            FindIfPlayInRange();
        }
    }

    private void FindIfPlayInRange()
    {
        BoxCollider2D playerCollider;
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        if (boxCollider2D.IsTouching(playerCollider))
        {
            OpenHealthMenu();
        }
    }

    public void OnMaxClick()
    {
        healAmount = missingHealth;
        healCost = missingHealth;
        if(healAmount > playerStats.Gold)
        {
            healAmount = playerStats.Gold;
            healCost = playerStats.Gold;
        }
        healAmountText.text = healAmount.ToString();
        healCostText.text = healCost.ToString();
    }

    public void OnPlusAmountClick()
    {
        if (healAmount < missingHealth)
        {
            if(healAmount < missingHealth - 10)
            {
                healAmount += 10;
                healCost += 10;
            }
            else
            {
                healAmount += missingHealth - healAmount;
                healCost += missingHealth - healCost;
            }
            
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

            //make sure the healAmount/Cost doesn't go below 0
            if(healAmount < 0)
            {
                healAmount = 0;
                healCost = 0;
            }
            healAmountText.text = healAmount.ToString();
            healCostText.text = healCost.ToString();
        }
    }

    public void OnHealClick()
    {
        if (playerStats.Gold >= healCost)
        {
            playerStats.ApplyHeal(healAmount);
            playerStats.Gold -= healCost;
            healthMenu.SetActive(false);
        }
    }

}
