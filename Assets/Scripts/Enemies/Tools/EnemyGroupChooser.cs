using UnityEngine;

public class EnemyGroupChooser : MonoBehaviour
{
    private MapType mapType;
    private RoomType roomType;
    private string roomKey;
    private int chosenChild;

    private void Start()
    {
        mapType = MapManager.Instance.CurrentMap;
        roomKey = MapManager.Instance.CurrentMapCoords.x.ToString() + MapManager.Instance.CurrentMapCoords.y.ToString();
        roomType = MapManager.Instance.GetMapRoomType();
        ChooseGroup();
    }

    // Use this for initialization
    private void ChooseGroup()
    {
        //if(roomType == RoomType.bossRoom)
        //{
        //    if(ES3.FileExists("Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + mapType))
        //    {
        //        GameObject child = transform.GetChild(0).gameObject;
        //        EnemiesSerializer childSerializer = child.GetComponent<EnemiesSerializer>();
        //        childSerializer.MapType = mapType;
        //        childSerializer.RoomKey = roomKey;
        //        child.SetActive(true);
        //        childSerializer.ReloadEnemies();
        //    }
        //    else
        //    {
        //        GameObject child = transform.GetChild(0).gameObject;
        //        EnemiesSerializer childSerializer = child.GetComponent<EnemiesSerializer>();
        //        childSerializer.MapType = mapType;
        //        childSerializer.RoomKey = roomKey;
        //        child.SetActive(true);
        //        childSerializer.LoadEnemies();
        //    }
        //}

        if (ES3.FileExists("Levels/" + mapType + "/Room" + roomKey + "/Enemies"))
        {
            chosenChild = ES3.Load<int>("ChosenGroup", "Levels/" + mapType + "/Room" + roomKey + "/Enemies");
            GameObject chosenGroup = transform.GetChild(chosenChild).gameObject;
            chosenGroup.SetActive(true);
            //EnemiesSerializer chosenGroupSerializer = chosenGroup.GetComponent<EnemiesSerializer>();
            //chosenGroupSerializer.MapType = mapType;
            //chosenGroupSerializer.RoomKey = roomKey;
            //chosenGroupSerializer.ReloadEnemies();
        }
        else
        {
            chosenChild = Random.Range(0, transform.childCount);
            GameObject chosenGroup = transform.GetChild(chosenChild).gameObject;
            chosenGroup.SetActive(true);
            //EnemiesSerializer chosenGroupSerializer = chosenGroup.GetComponent<EnemiesSerializer>();
            //chosenGroupSerializer.MapType = mapType;
            //chosenGroupSerializer.RoomKey = roomKey;
            //chosenGroupSerializer.LoadEnemies();
        }
    }

    private void OnDestroy()
    {
        ES3.Save<int>("ChosenGroup", chosenChild, "Levels/" + mapType + "/Room" + roomKey + "/Enemies");
    }
}
