using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform ChosenCharacter => characters.GetChild(0);

    public bool IsNewGame { get; private set; } = true;

    [SerializeField]
    private Transform characters;
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

    public void ChooseCharacter(CharacterClass characterClass, bool isNewGame)
    {
        chosenCharacterClass = characterClass;
        IsNewGame = isNewGame;

        for (int i = 0; i < characters.childCount; i++)
        {
            var child = characters.GetChild(i);
            if (child.GetComponent<PlayerStats>().CharacterClass != chosenCharacterClass)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // private void OnApplicationQuit() {}
}
