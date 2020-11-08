using UnityEngine;
using Factories;

public class Resume : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    public void PressResume()
    {
        menu.SetActive(false);
        GameStateManager.GameState = GameStateEnum.unpaused;
    }

    public void ResumeForMenuFactory()
    {
        MenuFactory.DestroyMenuAndUnpause();
    }

}
