using UnityEngine;

public class SpawnManagerForestTier1 : Spawn
{

    private float TimePassed;
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    private GameObject[] AliveEnemies;


    // Use this for initialization
    void Start ()
    {
        DifficultyIncreaseDelay = C.DIFFICULTY_INCREASE_DELAY;
        switch(GameHandler.CurrentLevel)
        {
            case 1:
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
                break;
            case 2:
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.CrowPrefab, C.CROW_SPAWN_FREQUENCY, 3));
                break;
            case 3:
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.CrowPrefab, C.CROW_SPAWN_FREQUENCY, 3));
                break;
            case 4:
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.CrowPrefab, C.CROW_SPAWN_FREQUENCY, 3));
                break;
            case 5:
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.CrowPrefab, C.CROW_SPAWN_FREQUENCY, 3));
                StartCoroutine(SpawnGroundEnemy<Wolf>(C.WOLF_SPAWN_FREQUENCY, 7, SpawnPoint1, SpawnPoint2, ObjectFactory.Instance.WolfPrefab));
                break;
            default:
                StartCoroutine(SpawnFlyingEnemy(ObjectFactory.Instance.BatPrefab, C.BAT_SPAWN_FREQUENCY, 1));
                break;
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {

        TimePassed += Time.deltaTime;

        if(TimePassed > 10f)
        {
            StopAllCoroutines();

            AliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if(AliveEnemies.Length == 0)
            {
                GameHandler.Instance.CallVictoryMenu();
            }
        }
	}
}
