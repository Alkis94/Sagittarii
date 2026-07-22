using UnityEngine;

public class BossEnemyLoader : EnemyLoader
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Load()
    {
        Vector3 originalPosition = transform.position;
        enemyStats = GetComponent<EnemyStats>();
        enemyStats.LoadFromData();
        dead = ES3.Load<bool>("Dead" + EnemyKey, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + enemyStats.EnemyName);
        criticalDeath = ES3.Load<bool>("CriticalDeath" + EnemyKey, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + enemyStats.EnemyName);
        transform.SetPositionAndRotation(
            ES3.Load<Vector3>("Position" + EnemyKey, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + enemyStats.EnemyName), 
            ES3.Load<Quaternion>("Rotation" + EnemyKey, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + enemyStats.EnemyName));
        GetComponent<EnemyBrain>().LoadEnemyBrain(originalPosition, dead);
        GetComponent<BossHealth>().enabled = false;
    }

    public override void ChangeEnemyStatusToDead(bool criticalDeath)
    {
        dead = true;
        this.criticalDeath = criticalDeath;
        ES3.Save("Dead" + EnemyKey, dead, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + enemyStats.EnemyName);
        ES3.Save("CriticalDeath" + EnemyKey, criticalDeath, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + enemyStats.EnemyName);
        ES3.Save("Position" + EnemyKey, transform.position, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + enemyStats.EnemyName);
        ES3.Save("Rotation" + EnemyKey, transform.rotation, ProfileManager.Instance.GetProfileRunPath() + SaveFolders.Bosses + "/" + enemyStats.EnemyName);
    }
}
