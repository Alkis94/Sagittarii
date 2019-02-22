using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{

    public void PressRestart()
    {
        PlayerStats.ChangePlayerCurrentHealth(PlayerStats.MaximumHealth);
        var thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
        Destroy(transform.parent.gameObject);
        GameState.UnpauseGame();
    }


}
