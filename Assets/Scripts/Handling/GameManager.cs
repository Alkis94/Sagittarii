﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static GameStateEnum gameState = GameStateEnum.unpaused;

    public static GameStateEnum GameState
    {
        get => gameState;

        set
        {
            gameState = value;

            if(gameState == GameStateEnum.paused)
            {
                Time.timeScale = 0;
            }
            else if (gameState == GameStateEnum.unpaused)
            {
                Time.timeScale = 1;
            }
            else if (gameState == GameStateEnum.slowed)
            {
                Time.timeScale = 0.5f;
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnApplicationQuit()
    {
        ES3.DeleteDirectory("Levels/");
    }
}
