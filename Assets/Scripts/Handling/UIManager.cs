using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{

    private int Score;


    public Text HealthText;
    public Text ScoreText;
    public Text MuteText;
    // public Text TimeText;


    private void OnEnable()
    {
        PlayerCollision.OnPlayerHealthChanged+= UpdateHealth;
    }

    private void OnDisable()
    {
        PlayerCollision.OnPlayerHealthChanged -= UpdateHealth;
    }

    private void Start ()
    {
        
        AudioListener.pause = false;
        Score = 0;
        HealthText.text =  PlayerStats.CurrentHealth + "/" + PlayerStats.MaximumHealth;

    }


    private void UpdateHealth()
    {
        int health;
        health = 0 > PlayerStats.CurrentHealth ? 0 : PlayerStats.CurrentHealth;
        health = PlayerStats.MaximumHealth < health ? PlayerStats.MaximumHealth : health;
        HealthText.text =  health.ToString() + "/" + PlayerStats.MaximumHealth.ToString();
    }



    //public void UpdateScore()
    //{
    //    Score += 10;
    //    ScoreText.text = "Score: " + Score.ToString();
    //}

    public void PressMute()
    {
        if (AudioListener.pause)
        {
            AudioListener.pause = false;
            MuteText.text = "Mute : Off";
        }
        else
        {
            AudioListener.pause = true;
            MuteText.text = "Mute : On";
        }
    }
}
