using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PressMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Destroy(transform.parent.gameObject);
    }
}
