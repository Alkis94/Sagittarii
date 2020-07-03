using UnityEngine;
using System.Collections;

public class EnemyGroupChooser : MonoBehaviour
{
    private MapType mapType;
    private RoomType roomType;
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
    private void ChooseGroup()
    {
        if(roomType == RoomType.bossRoom)
        {
            if(ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + mapType))
            {
                GameObject child = transform.GetChild(0).gameObject;
                EnemiesSerializer childSerializer = child.GetComponent<EnemiesSerializer>();
                childSerializer.MapType = mapType;
                childSerializer.RoomKey = roomKey;
                child.SetActive(true);
                childSerializer.ReloadEnemies();
            }
            else
            {
                GameObject child = transform.GetChild(0).gameObject;
                EnemiesSerializer childSerializer = child.GetComponent<EnemiesSerializer>();
                childSerializer.MapType = mapType;
                childSerializer.RoomKey = roomKey;
                child.SetActive(true);
                childSerializer.LoadEnemies();
            }
            
        }
        else
        {
            if (ES3.FileExists("Levels/" + mapType + "/Room" + roomKey))
            {
                chosenChild = ES3.Load<int>("ChosenGroup", "Levels/" + mapType + "/Room" + roomKey);
                GameObject chosenGroup = transform.GetChild(chosenChild).gameObject;
                EnemiesSerializer chosenGroupSerializer = chosenGroup.GetComponent<EnemiesSerializer>();
                chosenGroupSerializer.MapType = mapType;
                chosenGroupSerializer.RoomKey = roomKey;
                chosenGroup.SetActive(true);
                chosenGroupSerializer.ReloadEnemies();
            }
            else
            {
                chosenChild = Random.Range(0, transform.childCount);
                GameObject chosenGroup = transform.GetChild(chosenChild).gameObject;
                EnemiesSerializer chosenGroupSerializer = chosenGroup.GetComponent<EnemiesSerializer>();
                chosenGroupSerializer.MapType = mapType;
                chosenGroupSerializer.RoomKey = roomKey;
                chosenGroup.SetActive(true);
                chosenGroupSerializer.LoadEnemies();
            }
        }
    }

    private void GetInfoAndChoose(MapType mapType, string roomKey, RoomType roomType)
    {
        this.mapType = mapType;
        this.roomKey = roomKey;
        this.roomType = roomType;
        ChooseGroup();
    }

    private void OnDestroy()
    {
        MapManager.OnRoomLoaded -= GetInfoAndChoose;
        ES3.Save<int>("ChosenGroup", chosenChild, "Levels/" + mapType + "/Room" + roomKey);
    }
}
