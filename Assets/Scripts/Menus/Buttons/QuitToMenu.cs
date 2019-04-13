using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMenu : MonoBehaviour
{
    private GameObject DontDestroyOnLoadObject;

    public void LoadMainMenu()
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
