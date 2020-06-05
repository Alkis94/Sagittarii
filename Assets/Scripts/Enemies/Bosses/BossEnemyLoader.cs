using UnityEngine;
using System.Collections;

public class BossEnemyLoader : EnemyLoader
{
    //returns if the enemy is loaded dead (true) or alive (false)
    public override void Load()
    {
        Vector3 originalPosition = transform.position;
        dead = ES3.Load<bool>("Dead" + EnemyKey, "Bosses/" + MapType);
        criticalDeath = ES3.Load<bool>("CriticalDeath" + EnemyKey, "Bosses/" + MapType);
        transform.position = ES3.Load<Vector3>("Position" + EnemyKey, "Bosses/" + MapType);
        transform.rotation = ES3.Load<Quaternion>("Rotation" + EnemyKey, "Bosses/" + MapType);
        GetComponent<EnemyBrain>().LoadEnemyBrain(originalPosition, dead);
        GetComponent<BossHealth>().enabled = false;
    }

    public override void ChangeEnemyStatusToDead(bool criticalDeath)
    {
        dead = true;
        this.criticalDeath = criticalDeath;
    }

    private void OnDestroy()
    {
        ES3.Save<bool>("Dead" + EnemyKey, dead, "Bosses/" + MapType);
        ES3.Save<bool>("CriticalDeath" + EnemyKey, criticalDeath, "Bosses/" + MapType);
        ES3.Save<Vector3>("Position" + EnemyKey, transform.position, "Bosses/" + MapType);
        ES3.Save<Quaternion>("Rotation" + EnemyKey, transform.rotation, "Bosses/" + MapType);
    }
}
