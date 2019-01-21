using UnityEngine;

public class SpawnManagerEndless : Spawn
{ 

    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public Transform SpawnPoint3;
    public Transform SpawnPoint4;


    void Start()
    {
        DifficultyIncreaseDelay = C.DIFFICULTY_INCREASE_DELAY;
        //StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1,SpawnPoint3,SpawnPoint4));
        //StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.CrowPrefab, C.CROW_SPAWN_FREQUENCY, 3,SpawnPoint3,SpawnPoint4));
        //StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.MedusaPrefab, C.MEDUSA_SPAWN_FREQUENCY, C.MEDUSA_SPAWN_FREQUENCY));
        //StartCoroutine(SpawnGroundEnemy<Imp>(C.IMP_SPAWN_FREQUENCY, 10, SpawnPoint1, SpawnPoint2,ObjectFactory.Instance.ImpPrefab));
        //StartCoroutine(SpawnGroundEnemy<Wolf>(C.WOLF_SPAWN_FREQUENCY, 7, SpawnPoint1, SpawnPoint2, ObjectFactory.Instance.WolfPrefab));
        //SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1);
    }


}
