using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    [SerializeField]
    private Canvas quitMenu;
    [SerializeField]
    private Button playButton;


    void Start()
    {
        quitMenu.enabled = false;
    }


    public void QuitPress()
    {
        quitMenu.enabled = true;
        playButton.enabled = false;
        GetComponent<Button>().enabled = false;
    }

    public void YesPress()
    {
        Application.Quit();
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        playButton.enabled = true;
        GetComponent<Button>().enabled = true;
    }


}
