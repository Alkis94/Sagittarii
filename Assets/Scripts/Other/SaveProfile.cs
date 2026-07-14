using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveProfile : MonoBehaviour
{
    [SerializeField]
    private int profileId;
    [SerializeField]
    private GameObject chooseHeroMenu;
    [SerializeField]
    private GameObject saveMenu;
    [SerializeField]
    private GameObject newGameText;
    [SerializeField]
    private GameObject characterImage;
    [SerializeField]
    private List<Sprite> heroSprites;

    private bool isNew = true;
    private int hero;

    private void Start()
    {
        if(ES3.FileExists(SaveFolders.SaveProfile + profileId + SaveFolders.PlayerStats))
        {
            isNew = false;
            newGameText.SetActive(false);
            characterImage.SetActive(true);
            hero = ES3.Load<int>("Class", SaveManager.Instance.GetProfileRunPath() + SaveFolders.PlayerStats);
            Image heroImage = characterImage.GetComponent<Image>();
            heroImage.sprite = heroSprites[hero];
            heroImage.GetComponent<Image>().SetNativeSize();
        }
    }

    public void OnStartPress()
    {
        SaveManager.Instance.SetSaveId(profileId);

        if(isNew)
        {
            chooseHeroMenu.SetActive(true);
            saveMenu.SetActive(false);
        }
        else
        {
            GameManager.Instance.ChooseCharacter((CharacterClass)hero, false);
            SceneFader.Instance.LoadSceneWithFade(SceneNames.Town);
            UIManager.Instance.CallLocationText(LocationNames.Town);
        }
    }

    public void OnDeletePress()
    {
        if (ES3.DirectoryExists(SaveFolders.SaveProfile + profileId))
        {
            var heroImage = characterImage.GetComponent<Image>();
            heroImage.sprite = null;
            characterImage.SetActive(false);
            newGameText.SetActive(true);
            isNew = true;
            ES3.DeleteDirectory(SaveFolders.SaveProfile + profileId);
        }
    }
}
