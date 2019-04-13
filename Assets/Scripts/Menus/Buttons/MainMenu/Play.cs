using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Play : MonoBehaviour
{
    public void PlayPress()
    {
        SceneManager.LoadScene("Town");
        GameState.UnpauseGame();
    }
}
