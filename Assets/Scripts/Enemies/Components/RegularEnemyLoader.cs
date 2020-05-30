using UnityEngine;

public class RegularEnemyLoader : EnemyLoader
{ 
    //returns if the enemy is loaded dead (true) or alive (false)
    public override void Load()
    {
        Vector3 originalPosition = transform.position;
        dead = ES3.Load<bool>("Dead" + EnemyKey.ToString(), "Levels/" + MapType + "/Room" + RoomKey);
        criticalDeath = ES3.Load<bool>("CriticalDeath" + EnemyKey.ToString(), "Levels/" + MapType + "/Room" + RoomKey);
        transform.position = ES3.Load<Vector3>("Position" + EnemyKey.ToString(), "Levels/" + MapType + "/Room" + RoomKey);
        transform.rotation = ES3.Load<Quaternion>("Rotation" + EnemyKey.ToString(), "Levels/" + MapType + "/Room" + RoomKey);
        GetComponent<EnemyBrain>().LoadEnemyBrain(originalPosition, dead);
    }

    public override void ChangeEnemyStatusToDead(bool criticalDeath)
    {
        dead = true;
        this.criticalDeath = criticalDeath;
    }

    private void OnDestroy()
    {
        ES3.Save<bool>("Dead" + EnemyKey.ToString(), dead, "Levels/" + MapType + "/Room" + RoomKey);
        ES3.Save<bool>("CriticalDeath" + EnemyKey.ToString(), criticalDeath, "Levels/" + MapType + "/Room" + RoomKey);
        ES3.Save<Vector3>("Position" + EnemyKey.ToString(), transform.position, "Levels/" + MapType + "/Room" + RoomKey);
        ES3.Save<Quaternion>("Rotation" + EnemyKey.ToString(), transform.rotation, "Levels/" + MapType + "/Room" + RoomKey);
    }

}
