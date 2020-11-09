using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{

    public void PressRestart()
    {
        var thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
        GameManager.GameState = GameStateEnum.unpaused;
    }

}
