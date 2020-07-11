using UnityEngine;
using System.Collections;

public class BossEnemyLoader : EnemyLoader
{


    public override void Load()
    {
        Vector3 originalPosition = transform.position;
        dead = ES3.Load<bool>("Dead" + EnemyKey, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
        criticalDeath = ES3.Load<bool>("CriticalDeath" + EnemyKey, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
        transform.position = ES3.Load<Vector3>("Position" + EnemyKey, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
        transform.rotation = ES3.Load<Quaternion>("Rotation" + EnemyKey, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
        GetComponent<EnemyBrain>().LoadEnemyBrain(originalPosition, dead);
        GetComponent<BossHealth>().enabled = false;
    }

    public override void ChangeEnemyStatusToDead(bool criticalDeath)
    {
        dead = true;
        this.criticalDeath = criticalDeath;
        ES3.Save<bool>("Dead" + EnemyKey, dead, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
        ES3.Save<bool>("CriticalDeath" + EnemyKey, criticalDeath, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
        ES3.Save<Vector3>("Position" + EnemyKey, transform.position, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
        ES3.Save<Quaternion>("Rotation" + EnemyKey, transform.rotation, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
    }

    //private void OnDestroy()
    //{
    //    ES3.Save<bool>("Dead" + EnemyKey, dead, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
    //    ES3.Save<bool>("CriticalDeath" + EnemyKey, criticalDeath, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
    //    ES3.Save<Vector3>("Position" + EnemyKey, transform.position, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
    //    ES3.Save<Quaternion>("Rotation" + EnemyKey, transform.rotation, "Saves/Profile" + SaveProfile.SaveID + "/Bosses/" + MapType);
    //}
}
