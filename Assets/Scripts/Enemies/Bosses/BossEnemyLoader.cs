using UnityEngine;
using System.Collections;

public class BossEnemyLoader : EnemyLoader
{
    //returns if the enemy is loaded dead (true) or alive (false)
    public override void Load()
    {
        Vector3 originalPosition = transform.position;
        dead = ES3.Load<bool>("Dead", "Bosses/" + MapType);
        criticalDeath = ES3.Load<bool>("CriticalDeath", "Bosses/" + MapType);
        transform.position = ES3.Load<Vector3>("Position", "Bosses/" + MapType);
        transform.rotation = ES3.Load<Quaternion>("Rotation", "Bosses/" + MapType);
        GetComponent<EnemyBrain>().LoadEnemyBrain(originalPosition, dead);
    }

    public override void ChangeEnemyStatusToDead(bool criticalDeath)
    {
        dead = true;
        this.criticalDeath = criticalDeath;
    }

    private void OnDestroy()
    {
        ES3.Save<bool>("Dead", dead, "Bosses/" + MapType);
        ES3.Save<bool>("CriticalDeath", criticalDeath, "Bosses/" + MapType);
        ES3.Save<Vector3>("Position", transform.position, "Bosses/" + MapType);
        ES3.Save<Quaternion>("Rotation", transform.rotation, "Bosses/" + MapType);
    }
}
