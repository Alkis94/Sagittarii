using UnityEngine;
using Factories;
using System;

public class Resume : MonoBehaviour
{

    public event Action OnResumePressed = delegate { };

    public void PressResume()
    {
        GameState.UnpauseGame();
        OnResumePressed?.Invoke();
    }

    public void ResumeForMenuFactory()
    {
        MenuFactory.DestroyMenuAndUnpause();
    }
}
