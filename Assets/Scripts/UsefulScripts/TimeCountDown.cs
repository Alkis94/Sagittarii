using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;


public class TimeCountDown : MonoBehaviour
{
    private int TimeLimit = 600;
    private TextMeshProUGUI timeText;
    private PlayerStats playerStats;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Use this for initialization
    void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        timeText.enabled = false;
    }

    IEnumerator CountDown()
    {
        float timeLeft = TimeLimit;
        string minutes;
        string seconds = "0";

        while (true)
        {

            timeLeft -= Time.deltaTime;
            minutes = Mathf.Floor(timeLeft / 60).ToString("00");
            seconds = Mathf.Floor(timeLeft % 60).ToString("00");
            timeText.text = minutes + ":" + seconds;

            if (timeLeft <= 0)
            {
                timeText.text = "00:00";
                playerStats.ApplyDamage(playerStats.MaximumHealth, DamageSource.judgement);
                break;
            }

            yield return null;

        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            StopAllCoroutines();
            timeText.enabled = false;
        }
        else
        {
            if(!timeText.enabled)
            {
                if (playerStats == null)
                {
                    playerStats = FindObjectOfType<PlayerStats>();
                }
                TimeLimit = playerStats.TimeLimit;
                timeText.enabled = true;
                StartCoroutine(CountDown());
            }
        }
    }
}

