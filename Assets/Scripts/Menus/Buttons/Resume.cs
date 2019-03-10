using UnityEngine;
using Factories;
using System;

public class Resume : MonoBehaviour
{

    public event Action OnResumePressed = delegate { };

    public void PressResume()
    {
        Debug.Log("Resume Pressed");
        GameState.UnpauseGame();
        OnResumePressed?.Invoke();
    }
}
