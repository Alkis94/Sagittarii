using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChooseHero : MonoBehaviour
{
    [SerializeField]
    private CharacterClass characterClass;

    public void OnChoosePress()
    {
        FindObjectOfType<CharacterChooser>().CharacterClassChosen = characterClass;
        SceneFader.Instance.LoadSceneWithFade("Town");
        UIManager.Instance.CallLocationText("Floyd's Rest");
    }
}
