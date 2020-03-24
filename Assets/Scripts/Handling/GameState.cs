using UnityEngine;
using System;
using System.Collections;

public static class GameState
{

    public static bool GamePaused { get; private set; } = false;

    
    public static void PauseGame()
    {
        GamePaused = true;
        Time.timeScale = 0;
    }

    public static void UnpauseGame()
    {
        GamePaused = false;
        Time.timeScale = 1;
    }

}
