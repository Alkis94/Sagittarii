using UnityEngine;
using System.Collections;

public class TaxHandler : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField]
    private int Tax = 100;
    public bool taxWasPaid = false;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public void OnPayPressed()
    {
        if(playerStats.Gold >= Tax)
        {
            playerStats.Gold -= Tax;
            taxWasPaid = true;
            gameObject.SetActive(false);
        }   
    }

    public void OnDontPayPressed()
    {
        gameObject.SetActive(false);
    }

}
