using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject quitMenu;
    [SerializeField]
    private Transform mainButtons;

    private void Start()
    {
        quitMenu.SetActive(false);
    }

    public void OnPlayPress()
    {
        SceneManager.LoadScene("Town");
        GameState.UnpauseGame();
    }

    public void OnQuitPress()
    {
        quitMenu.SetActive(true);
        foreach (Transform button in mainButtons)
        {
            button.GetComponent<Button>().enabled = false;
        }
    }

    public void OnQuitYesPress()
    {
        Application.Quit();
    }

    public void OnQuitNoPress()
    {
        quitMenu.SetActive(false);
        foreach (Transform button in mainButtons)
        {
            button.GetComponent<Button>().enabled = true;
        }
    }

}
