using UnityEngine;
using System.Collections;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField]
    private float upperLimit = 8.5f;
    [SerializeField]
    private float lowerLimit = 3.5f;
    [SerializeField]
    private float spawnFrequency = 30f;
    [SerializeField]
    private float spawnRandomness = 10f;
    [SerializeField]
    private bool flip;


    [SerializeField]
    private GameObject bird;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {

        float randomDelay = Random.Range(0f, 3f);


        while (true)
        {
            yield return new WaitForSeconds(randomDelay);
            randomDelay = Random.Range(spawnFrequency, spawnFrequency + spawnRandomness);
            SpawnCLoud();
        }
    }

    private GameObject SpawnCLoud()
    {
        Vector3 randomPosition = new Vector3(transform.position.x, Random.Range(lowerLimit, upperLimit), 0);
        GameObject tempbird = Instantiate(bird, randomPosition, Quaternion.identity);
        if (flip) tempbird.transform.localRotation = Quaternion.Euler(0, 180, 0);
        return tempbird;
    }
}
