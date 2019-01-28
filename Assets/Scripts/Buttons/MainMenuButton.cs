using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void PressMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Destroy(transform.parent.gameObject);
    }
}
