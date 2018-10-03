using UnityEngine;
using UnityEngine.SceneManagement;

public class TownTreeButton : MonoBehaviour
{

	public void PressTreeButton()
    {
        SceneManager.LoadScene("ForestTier1");
    }
}
