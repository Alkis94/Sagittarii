using UnityEngine;
using System.Collections;

public class EnemyGroupChooser : MonoBehaviour
{
    private MapType mapType;
    private string roomKey;
    private int chosenChild;


    private void OnEnable()
    {
        MapManager.OnRoomLoaded += GetInfoAndChoose;
    }

    private void OnDisable()
    {
        MapManager.OnRoomLoaded -= GetInfoAndChoose;
    }

    // Use this for initialization
    void ChooseGroup()
    {
        if (ES3.FileExists("Levels/" + mapType + "/Room" + roomKey))
        {
            chosenChild = ES3.Load<int>("ChosenGroup", "Levels/" + mapType + "/Room" + roomKey);
            GameObject chosenGroup = transform.GetChild(chosenChild).gameObject;
            chosenGroup.GetComponent<EnemiesSerializer>().MapType = mapType;
            chosenGroup.GetComponent<EnemiesSerializer>().RoomKey = roomKey;
            chosenGroup.SetActive(true);
        }
        else
        {
            chosenChild = Random.Range(0, transform.childCount);
            GameObject chosenGroup = transform.GetChild(chosenChild).gameObject;
            chosenGroup.GetComponent<EnemiesSerializer>().MapType = mapType;
            chosenGroup.GetComponent<EnemiesSerializer>().RoomKey = roomKey;
            chosenGroup.SetActive(true);
            
        }
    }

    private void OnDestroy()
    {
        ES3.Save<int>("ChosenGroup", chosenChild, "Levels/" + mapType + "/Room" + roomKey);
    }

    private void GetInfoAndChoose(MapType mapType, string roomKey)
    {
        this.mapType = mapType;
        this.roomKey = roomKey;
        ChooseGroup();
    }
}
