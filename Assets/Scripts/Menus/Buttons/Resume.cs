using UnityEngine;
using Factories;
using System;

public class Resume : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    public void PressResume()
    {
        DestroyMenu();
        GameState.UnpauseGame();
    }

    public void ResumeForMenuFactory()
    {
        MenuFactory.DestroyMenuAndUnpause();
    }

    private void DestroyMenu()
    {
        Destroy(menu);
    }

}
