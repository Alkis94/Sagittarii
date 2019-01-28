using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    //   # # # # # # # # # # # # 
    //   #                     #
    //   #  SINGLETON CLASS    #
    //   #                     #
    //   # # # # # # # # # # # # 

    public static GameHandler Instance = null;


    public static int CurrentLevel = 1;


    public Text PausedMenuText;
    public Text ContinueText;
    public Button ContinueButton;
    public Canvas PausedMenu;
    public Canvas VictoryMenu;

    



    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        Instance = this;
        //Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }

    void Start()
    {
        Time.timeScale = 1;
        GameState.GamePaused = false;
        PausedMenu.enabled = false;
        VictoryMenu.enabled = false;
    }

    void Update ()
    {
        if (Input.GetButtonDown("Cancel") && !GameState.GamePaused)
        {
            CallPausedMenu();
        }
        else if (Input.GetButtonDown("Cancel") && GameState.GamePaused)
        {
            UnpauseGame();
        }
    }

    public void CallVictoryMenu()
    {
        VictoryMenu.enabled = true;
        Time.timeScale = 0;
    }

    public void CallPlayerDiedMenu()
    {
        PausedMenu.enabled = true;
        PausedMenuText.text = "You died!";
        ContinueButton.enabled = false;
        ContinueText.enabled = false;
    }

    public void CallPausedMenu()
    {
        PausedMenu.enabled = true;
        PausedMenuText.text = "Paused";
        Time.timeScale = 0;
        GameState.GamePaused = true;
    }

    public void UnpauseGame()
    {
        PausedMenu.enabled = false;
        Time.timeScale = 1;
        GameState.GamePaused = false;
    }

    public void PressRestart()
    {
        var thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }

    public void PressMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PressVictoryContinue()
    {
        if(CurrentLevel > 4)
        {
            SceneManager.LoadScene("Town");
            CurrentLevel = 1;
        }
        else
        {
            CurrentLevel += 1;
            var thisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(thisScene.name);
        }
    }
}
