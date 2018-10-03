using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public Canvas QuickMenu;
    public Button PlayButton;
    public Button QuitButton;

	void Start ()
    {
        QuickMenu.enabled = false;
        Time.timeScale = 1;
    }
	
    public void PlayPress()
    {
        SceneManager.LoadScene("LevelEndless");
    }

    public void QuitPress()
    {
        QuickMenu.enabled = true;
        PlayButton.enabled = false;
        QuitButton.enabled = false;
    }

    public void YesPress()
    {
        Application.Quit();
    }

    public void NoPress()
    {
        QuickMenu.enabled = false;
        PlayButton.enabled = true;
        QuitButton.enabled = true;
    }
}
