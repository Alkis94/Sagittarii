using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;


public class TimeCountDown : MonoBehaviour
{
    public int TimeLimit { get; set; } = 10;
    private TextMeshProUGUI timeText;

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
        float timeLeft = TimeLimit * 60;
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
                PlayerStats playerStats = FindObjectOfType<PlayerStats>();
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
                timeText.enabled = true;
                StartCoroutine(CountDown());
            }
        }
    }
}

