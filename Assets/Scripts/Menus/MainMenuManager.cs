using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager Instance = null;

    [SerializeField]
    private GameObject quitMenu;
    [SerializeField]
    private GameObject howToPlayMenu;
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private GameObject profilesMenu;
    [SerializeField]
    private GameObject chooseHeroMenu;

    [SerializeField]
    private Transform mainButtons;

    [SerializeField]
    private RectTransform howToPlayButton;
    [SerializeField]
    private GameObject continueButton;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject abandonRunButton;

    [SerializeField]
    private TextMeshProUGUI chosenProfile;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        ChooseProfile.OnProfileChanged += ChangeButtonsAndText;
    }

    private void OnDisable()
    {
        ChooseProfile.OnProfileChanged -= ChangeButtonsAndText;
    }

    private void Start()
    {
        ChangeButtonsAndText();
    }

    public void OnHowToPlayPress()
    {
        howToPlayMenu.SetActive(!howToPlayMenu.activeInHierarchy);
    }

    public void OnStartPress()
    {
        chooseHeroMenu.SetActive(!chooseHeroMenu.activeInHierarchy);
    }

    public void OnContinuePress()
    {
        GameManager.Instance.ChooseCharacter(false);
        SceneFader.Instance.LoadSceneWithFade(SceneNames.Town);
        UIManager.Instance.ShowLocation(LocationNames.Town);
    }

    public void OnAbandonRunPress()
    {
        ProfileManager.Instance.DeleteRun();
        continueButton.SetActive(false);
        abandonRunButton.SetActive(false);
        startButton.SetActive(true);
        howToPlayButton.anchoredPosition = new Vector2(howToPlayButton.anchoredPosition.x, -20f);
    }

    public void OnOptionsPress()
    {
        optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
    }

    public void OnFeedbackPress()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSeh8mK7OmkDzpzj1dnIaeTQ7_w8CZTfgyQgHpa0gMru9Ud9ow/viewform");
    }

    public void OnProfilesPress()
    {
        profilesMenu.SetActive(!profilesMenu.activeInHierarchy);
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

    private void ChangeButtonsAndText()
    {
        chosenProfile.text = "Profile " + ProfileManager.Instance.ProfileId;

        if (ProfileManager.Instance.ProfileHasActiveRun())
        {
            continueButton.SetActive(true);
            abandonRunButton.SetActive(true);
            startButton.SetActive(false);
            howToPlayButton.anchoredPosition = new Vector2(howToPlayButton.anchoredPosition.x, 0f);
        }
        else
        {
            continueButton.SetActive(false);
            abandonRunButton.SetActive(false);
            startButton.SetActive(true);
            howToPlayButton.anchoredPosition = new Vector2(howToPlayButton.anchoredPosition.x, -20f);
        }

        profilesMenu.SetActive(false);
    }
}
