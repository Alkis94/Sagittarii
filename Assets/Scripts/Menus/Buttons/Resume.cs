using UnityEngine;
using Factories;

public class Resume : MonoBehaviour
{
    public void PressResume()
    {
        Debug.Log("Resume Pressed");
        GameState.UnpauseGame();
        Destroy(transform.parent.gameObject);
    }
}
