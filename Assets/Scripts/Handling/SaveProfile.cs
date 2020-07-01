using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SaveProfile : MonoBehaviour
{
    public static int SaveID { get; private set; } = 1;


    [SerializeField]
    private int profileID;
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

    // Use this for initialization
    private void Start()
    {
        if(ES3.FileExists("Saves/Profile" + profileID + "/PlayerStats"))
        {
            isNew = false;
            newGameText.SetActive(false);
            characterImage.SetActive(true);
            hero = ES3.Load<int>("Class", "Saves/Profile" + profileID + "/PlayerStats");
            Image heroImage = characterImage.GetComponent<Image>();
            heroImage.sprite = heroSprites[hero];
            heroImage.GetComponent<Image>().SetNativeSize();
        }
    }

    public void OnStartPress()
    {
        SaveID = profileID;

        if(isNew)
        {
            chooseHeroMenu.SetActive(true);
            saveMenu.SetActive(false);
        }
        else
        {
            FindObjectOfType<CharacterChooser>().CharacterClassChosen = (CharacterClass)hero;
            SceneManager.LoadScene("Town");
        }
    }

    public void OnDeletePress()
    {
        if (ES3.DirectoryExists("Saves/Profile" + profileID))
        {
            Image heroImage = characterImage.GetComponent<Image>();
            heroImage.sprite = null;
            characterImage.SetActive(false);
            newGameText.SetActive(true);
            isNew = true;
            ES3.DeleteDirectory("Saves/Profile" + profileID);
        }
    }

   
}
