using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{

    public void PressRestart()
    {
        var thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
        Destroy(transform.parent.gameObject);
        GameState.UnpauseGame();
    }


}
