using UnityEngine;
using TMPro;

public class TaxHandler : MonoBehaviour
{
    public bool TaxWasPaid { get; private set; } = false;

    private PlayerStats playerStats;
    private int tax = 100;
    [SerializeField]
    private TextMeshProUGUI costText;
   

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        tax = playerStats.TownTax;
        costText.text = "Pay town entry tax " + tax;
    }

    public void OnPayPressed()
    {
        if(playerStats.Gold >= tax)
        {
            playerStats.Gold -= tax;
            TaxWasPaid = true;
            gameObject.SetActive(false);
        }   
    }

    public void OnDontPayPressed()
    {
        gameObject.SetActive(false);
    }

}
