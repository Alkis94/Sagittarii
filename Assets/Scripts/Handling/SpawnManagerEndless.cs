using UnityEngine;

public class SpawnManagerEndless : Spawn
{ 

    public Transform SpawnPoint1;
    public Transform SpawnPoint2;


    void Start()
    {
        DifficultyIncreaseDelay = C.DIFFICULTY_INCREASE_DELAY;
        StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
        StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.CrowPrefab, C.CROW_SPAWN_FREQUENCY, 3));
        StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.MedusaPrefab, C.MEDUSA_SPAWN_FREQUENCY, C.MEDUSA_SPAWN_FREQUENCY));
        StartCoroutine(SpawnGroundEnemy<Imp>(C.IMP_SPAWN_FREQUENCY, 10, SpawnPoint1, SpawnPoint2,ObjectFactory.Instance.ImpPrefab));
        StartCoroutine(SpawnGroundEnemy<Wolf>(C.WOLF_SPAWN_FREQUENCY, 7, SpawnPoint1, SpawnPoint2, ObjectFactory.Instance.WolfPrefab));
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
