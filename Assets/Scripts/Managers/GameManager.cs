using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Transform ChosenCharacter => characters.GetChild(0);
    public bool IsNewRun { get; private set; } = true;

    [SerializeField]
    private Transform characters;

    [SerializeField]
    private GameObject map;
    [SerializeField]
    private GameObject pauseMenu;

    private CharacterClass chosenCharacterClass = CharacterClass.None;
    private static GameStateEnum gameState = GameStateEnum.Unpaused;

    public static GameStateEnum GameState
    {
        get => gameState;

        set
        {
            gameState = value;

            if(gameState == GameStateEnum.Paused)
            {
                Time.timeScale = 0;
            }
            else if (gameState == GameStateEnum.Unpaused)
            {
                Time.timeScale = 1;
            }
            else if (gameState == GameStateEnum.Slowed)
            {
                Time.timeScale = 0.5f;
            }
        }
    }

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

        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnEnable()
    {
        PlayerInput.MapPressed += OnMapPress;
        PlayerInput.CancelPressed += OnCancelPress;
    }

    public void OnDisable()
    {
        PlayerInput.MapPressed -= OnMapPress;
        PlayerInput.CancelPressed -= OnCancelPress;
    }

    public void ChooseCharacter(bool isNewRun, CharacterClass characterClass = CharacterClass.None)
    {
        IsNewRun = isNewRun;

        if (isNewRun)
        {
            chosenCharacterClass = characterClass;
        }
        else
        {
            chosenCharacterClass = ES3.Load<CharacterClass>("Class", ProfileManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
        }

        for (var i = 0; i < characters.childCount; i++)
        {
            var child = characters.GetChild(i);
            if (child.GetComponent<PlayerStats>().CharacterClass != chosenCharacterClass)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void OnCancelPress()
    {
        if (map.activeInHierarchy)
        {
            GameState = GameStateEnum.Unpaused;
            map.SetActive(false);
        }
        else if (gameState == GameStateEnum.Unpaused)
        {
            GameState = GameStateEnum.Paused;
            pauseMenu.SetActive(true);
        }
        else if (gameState == GameStateEnum.Paused)
        {
            GameState = GameStateEnum.Unpaused;
            pauseMenu.SetActive(false);
        }
    }

    private void OnMapPress()
    {
        if (pauseMenu.activeInHierarchy)
        {
            return;
        }

        if (gameState == GameStateEnum.Unpaused)
        {
            GameState = GameStateEnum.Paused;
            map.SetActive(true);
        }
        else if (gameState == GameStateEnum.Paused)
        {
            GameState = GameStateEnum.Unpaused;
            map.SetActive(false);
        }
    }

    // private void OnApplicationQuit() {}
}
