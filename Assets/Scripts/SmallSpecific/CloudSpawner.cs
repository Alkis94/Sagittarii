using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField]
    private float upperLimit = 8.5f;
    [SerializeField]
    private float lowerLimit = 2.5f;
    [SerializeField]
    private float spawnFrequency = 7f;
    [SerializeField]
    private float spawnRandomness = 10f;
    [SerializeField]
    private int sortingLayerMin = 0;
    [SerializeField]
    private int sortingLayerMax = 5;

    [SerializeField]
    private GameObject cloud;

    [SerializeField]
    private Sprite cloud0;
    [SerializeField]
    private Sprite cloud1;
    [SerializeField]
    private Sprite cloud2;
    [SerializeField]
    private Sprite cloud3;

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
        GameObject tempCloud = Instantiate(cloud, randomPosition, Quaternion.identity);
        tempCloud.GetComponent<SpriteRenderer>().sprite = ReturnRandomSprite();
        tempCloud.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(sortingLayerMin, sortingLayerMax);
        return tempCloud;
    }

    private Sprite ReturnRandomSprite()
    {
        int randomInt = Random.Range(0, 3);

        switch(randomInt)
        {
            case 0:
                return cloud0;
            case 1:
                return cloud1;
            case 2:
                return cloud2;
            case 3:
                return cloud3;
            default:
                return cloud3;
        }
    }
}
