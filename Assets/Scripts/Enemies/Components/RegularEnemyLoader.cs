using UnityEngine;

public class RegularEnemyLoader : EnemyLoader
{

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Load()
    {
        Vector3 originalPosition = transform.position;
        dead = ES3.Load<bool>("Dead" + EnemyKey.ToString(), SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        criticalDeath = ES3.Load<bool>("CriticalDeath" + EnemyKey.ToString(), SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        transform.SetPositionAndRotation(
            ES3.Load<Vector3>("Position" + EnemyKey.ToString(), SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies), 
            ES3.Load<Quaternion>("Rotation" + EnemyKey.ToString(), SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies));
        GetComponent<EnemyBrain>().LoadEnemyBrain(originalPosition, dead);
    }

    public override void ChangeEnemyStatusToDead(bool criticalDeath)
    {
        dead = true;
        this.criticalDeath = criticalDeath;
    }

    private void OnDestroy()
    {
        ES3.Save("EnemyName" + EnemyKey.ToString(), enemyStats.EnemyName, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        ES3.Save("Dead" + EnemyKey.ToString(), dead, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        ES3.Save("CriticalDeath" + EnemyKey.ToString(), criticalDeath, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        ES3.Save("Position" + EnemyKey.ToString(), transform.position, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        ES3.Save("Rotation" + EnemyKey.ToString(), transform.rotation, SaveManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
    }
}
