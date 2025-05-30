﻿using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject quitMenu;
    [SerializeField]
    private GameObject howToPlayMenu;
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private Transform mainButtons;
    [SerializeField]
    private GameObject savesMenu;

    private void Start()
    {
        quitMenu.SetActive(false);
        if (ES3.DirectoryExists(("Levels/")))
        {
            ES3.DeleteDirectory("Levels/");
            ES3.DeleteDirectory("Levels/");
        }
    }

    public void OnPlayPress()
    {
        savesMenu.SetActive(!savesMenu.activeInHierarchy);
    }

    public void OnHowToPlayPress()
    {
        howToPlayMenu.SetActive(!howToPlayMenu.activeInHierarchy);
    }

    public void OnOptionsPress()
    {
        optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
    }

    public void OnFeedbackPress()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSeh8mK7OmkDzpzj1dnIaeTQ7_w8CZTfgyQgHpa0gMru9Ud9ow/viewform");
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
