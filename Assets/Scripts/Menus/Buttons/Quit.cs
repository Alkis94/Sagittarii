using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    private GameObject DontDestroyOnLoadObject;

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void QuitToMainMenu()
    {
        DestroyCurrentDontDestroy();
        SceneManager.LoadScene("MainMenu");
        GameStateManager.GameState = GameStateEnum.unpaused;
    }

    private void DestroyCurrentDontDestroy()
    {
        DontDestroyOnLoadObject = FindObjectOfType<Universal>().gameObject;
        Destroy(DontDestroyOnLoadObject);
    }
}
