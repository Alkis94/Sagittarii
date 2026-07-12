using UnityEngine;

public class ChooseHero : MonoBehaviour
{
    [SerializeField]
    private CharacterClass characterClass;

    public void OnChoosePress()
    {
        GameManager.Instance.ChooseCharacter(characterClass, true);
        SceneFader.Instance.LoadSceneWithFade("Town");
        UIManager.Instance.CallLocationText("Floyd's Rest");
    }
}
