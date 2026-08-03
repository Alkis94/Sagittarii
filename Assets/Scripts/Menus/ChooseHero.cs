using UnityEngine;

public class ChooseHero : MonoBehaviour
{
    [SerializeField]
    private CharacterClass characterClass;

    public void OnChoosePress()
    {
        GameManager.Instance.ChooseCharacter(true, characterClass);
        SceneFader.Instance.LoadSceneWithFade(SceneNames.Town);
        UIManager.Instance.ShowLocation(LocationNames.Town);
    }
}
