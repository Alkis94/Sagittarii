using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    [SerializeField]
    private Canvas QuitMenu;
    [SerializeField]
    private Button PlayButton;


    void Start()
    {
        QuitMenu.enabled = false;
    }


    public void QuitPress()
    {
        QuitMenu.enabled = true;
        PlayButton.enabled = false;
        GetComponent<Button>().enabled = false;
    }

    public void YesPress()
    {
        Application.Quit();
    }

    public void NoPress()
    {
        QuitMenu.enabled = false;
        PlayButton.enabled = true;
        GetComponent<Button>().enabled = true;
    }


}
