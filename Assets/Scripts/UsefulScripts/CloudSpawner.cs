using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{

    private const float upperLimit = 8.5f;
    private const float lowerLimit = 2.5f;
    
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
            randomDelay = Random.Range(5f, 12f);
            SpawnCLoud();
        }
    }

    private GameObject SpawnCLoud()
    {
        Vector3 randomPosition = new Vector3(transform.position.x, Random.Range(2.5f, 8.5f), 0);
        GameObject tempCloud = Instantiate(cloud, randomPosition, Quaternion.identity);
        tempCloud.GetComponent<SpriteRenderer>().sprite = ReturnRandomSprite();
        tempCloud.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(0, 4);
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
