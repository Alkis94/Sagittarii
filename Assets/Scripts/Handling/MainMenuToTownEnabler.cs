using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuToTownEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private GameObject character;
    [SerializeField]
    private GameObject playerCamera;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            UI.SetActive(true);
            playerCamera.SetActive(true);
            character.transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
