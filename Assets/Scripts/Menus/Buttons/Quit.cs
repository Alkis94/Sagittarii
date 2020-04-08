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
        GameState.UnpauseGame();
    }

    private void DestroyCurrentDontDestroy()
    {
        DontDestroyOnLoadObject = FindObjectOfType<DontDestroyOnLoadClass>().gameObject;
        Destroy(DontDestroyOnLoadObject);
    }
}
