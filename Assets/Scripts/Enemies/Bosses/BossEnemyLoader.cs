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
        dead = ES3.Load<bool>("Dead" + EnemyKey, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + enemyStats.EnemyName);
        criticalDeath = ES3.Load<bool>("CriticalDeath" + EnemyKey, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + enemyStats.EnemyName);
        transform.position = ES3.Load<Vector3>("Position" + EnemyKey, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + enemyStats.EnemyName);
        transform.rotation = ES3.Load<Quaternion>("Rotation" + EnemyKey, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + enemyStats.EnemyName);
        GetComponent<EnemyBrain>().LoadEnemyBrain(originalPosition, dead);
        GetComponent<BossHealth>().enabled = false;
    }

    public override void ChangeEnemyStatusToDead(bool criticalDeath)
    {
        dead = true;
        this.criticalDeath = criticalDeath;
        ES3.Save<bool>("Dead" + EnemyKey, dead, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + enemyStats.EnemyName);
        ES3.Save<bool>("CriticalDeath" + EnemyKey, criticalDeath, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + enemyStats.EnemyName);
        ES3.Save<Vector3>("Position" + EnemyKey, transform.position, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + enemyStats.EnemyName);
        ES3.Save<Quaternion>("Rotation" + EnemyKey, transform.rotation, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + enemyStats.EnemyName);
    }
}
