using UnityEngine;
using System.Collections;

public class TaxHandler : MonoBehaviour
{
    private PlayerStats playerStats;
    private int Tax = 100;
    public bool taxWasPaid = false;
    // Use this for initialization
    void Start()
    {
        GameManager.GameState = GameStateEnum.paused;
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public void OnPayPressed()
    {
        if(playerStats.Gold >= 100)
        {
            playerStats.Gold -= 100;
            taxWasPaid = true;
        }

        gameObject.SetActive(false);
        GameManager.GameState = GameStateEnum.unpaused;
    }

    public void OnDontPayPressed()
    {
        gameObject.SetActive(false);
        GameManager.GameState = GameStateEnum.unpaused;
    }

}
