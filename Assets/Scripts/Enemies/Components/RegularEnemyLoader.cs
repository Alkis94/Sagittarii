using UnityEngine;

public class RegularEnemyLoader : EnemyLoader
{

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Load()
    {
        var originalPosition = transform.position;
        dead = ES3.Load<bool>("Dead" + EnemyKey.ToString(), ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        criticalDeath = ES3.Load<bool>("CriticalDeath" + EnemyKey.ToString(), ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        transform.SetPositionAndRotation(
            ES3.Load<Vector3>("Position" + EnemyKey.ToString(), ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies), 
            ES3.Load<Quaternion>("Rotation" + EnemyKey.ToString(), ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies));
        GetComponent<EnemyBrain>().LoadEnemyBrain(originalPosition, dead);
    }

    public override void ChangeEnemyStatusToDead(bool criticalDeath)
    {
        dead = true;
        this.criticalDeath = criticalDeath;
    }

    private void OnDestroy()
    {
        if (!ProfileManager.Instance.ProfileHasActiveRun())
        {
            return;
        }

        ES3.Save("EnemyName" + EnemyKey.ToString(), enemyStats.EnemyName, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        ES3.Save("Dead" + EnemyKey.ToString(), dead, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        ES3.Save("CriticalDeath" + EnemyKey.ToString(), criticalDeath, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        ES3.Save("Position" + EnemyKey.ToString(), transform.position, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
        ES3.Save("Rotation" + EnemyKey.ToString(), transform.rotation, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Levels + "/" + MapType + SaveFolders.Room + RoomKey + SaveFolders.Enemies);
    }
}
