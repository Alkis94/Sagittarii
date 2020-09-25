using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemShop : MonoBehaviour, IInteractable
{

    [SerializeField]
    private GameObject itemMenu;
    [SerializeField]
    private List<GameObject> commonItems;
    [SerializeField]
    private List<GameObject> rareItems;
    private int rareItemID = -1;
    private GameObject[] chosenItems = new GameObject[3];
    private bool[] chosenItemIsRare = new bool[3];
    [SerializeField]
    private Transform []slots = new Transform [3];
    [SerializeField]
    private Button[] buyButtons = new Button[3];
    [SerializeField]
    private TextMeshProUGUI[] costText = new TextMeshProUGUI[3];
    private int[] cost = new int[3];
    [SerializeField]
    private GameObject[] soldGraphic = new GameObject[3];
    private PlayerStats playerStats;
    private AudioSource audioSource;
   

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        int count = rareItems.Count;
        for(int i = rareItems.Count - 1; i >= 0 ; i--)
        {
            if(ES3.KeyExists(rareItems[i].name, "Saves/Profile" + SaveProfile.SaveID + "/RareShopItemsBought"))
            {
                rareItems.Remove(rareItems[i]);
            }
        }

        bool rareItemNotChosen = true;
        for (int i=0; i < 3; i++)
        {
            float randomChance = Random.Range(0f, 1f);
            if(randomChance < 1f && rareItemNotChosen && rareItems.Count > 0)
            {
                rareItemNotChosen = false;
                SpawnItemInShop(i, rareItems, true);
                chosenItemIsRare[i] = true;
            }
            else
            {
                SpawnItemInShop(i,commonItems, false);
                chosenItemIsRare[i] = false;
            }
        }

        itemMenu.SetActive(false);
    }

    public void Interact()
    {
        OpenItemMenu();
    }

    private void OpenItemMenu()
    {
        itemMenu.SetActive(!itemMenu.activeSelf);

        if (playerStats == null)
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }
    }

    private GameObject SpawnItemInShop(int i,List<GameObject> items, bool isRare)
    {
        int randomNumber = Random.Range(0, items.Count);
        chosenItems[i] = items[randomNumber];
        if(isRare)
        {
            rareItemID = randomNumber;
        }
        GameObject item = Instantiate(items[randomNumber], slots[i].position, Quaternion.identity);
        item.transform.GetChild(1).gameObject.SetActive(false);
        cost[i] = item.transform.GetChild(2).GetComponent<ShopRelic>().Cost;
        costText[i].text = cost[i].ToString();
        item.transform.GetChild(2).gameObject.SetActive(false);
        item.transform.parent = slots[i];
        item.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        return item;
    }

    public void OnBuyPressed (int i)
    {
        if(playerStats.Gold >= cost[i])
        {
            audioSource.Play();
            playerStats.Gold -= cost[i];
            SpawnShopItem(i);
            soldGraphic[i].transform.SetAsLastSibling();
            soldGraphic[i].SetActive(true);
            buyButtons[i].interactable = false;
            if(chosenItemIsRare[i])
            {
                ES3.Save<string>(rareItems[rareItemID].name, rareItems[rareItemID].name, "Saves/Profile" + SaveProfile.SaveID + "/RareShopItemsBought");
                Debug.Log("Item : " + rareItems[rareItemID].name + " bought and saved!");
                rareItems.RemoveAt(rareItemID);
            }
        }
        
    }

    private GameObject SpawnShopItem(int i)
    {
        GameObject chosenItem = Instantiate(chosenItems[i], transform.position, Quaternion.identity); ;
        chosenItem.transform.GetChild(0).gameObject.SetActive(false);
        return chosenItem;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            itemMenu.SetActive(false);
        }
    }

}
