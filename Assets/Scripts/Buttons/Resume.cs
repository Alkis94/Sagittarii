using UnityEngine;
using Factories;

public class Resume : MonoBehaviour
{
    // Start is called before the first frame update
    public void PressResume()
    {
        Debug.Log("Resume Pressed");
        Time.timeScale = 1;
        GameState.GamePaused = false;
        Destroy(transform.parent.gameObject);
    }
}
