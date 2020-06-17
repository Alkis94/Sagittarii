using UnityEngine;
using System.Collections;

public class LeafSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject leaf;

    private void Start()
    {
        StartCoroutine(SpawnLeaf());
    }

    IEnumerator SpawnLeaf()
    {
        while(true)
        {
            float randomDelay = Random.Range(10f, 15f);
            yield return new WaitForSeconds(randomDelay);
            float randomX = Random.Range(-2, 2);
            Vector3 position = new Vector3(transform.position.x + randomX, transform.position.y, 0);
            Instantiate(leaf, position, Quaternion.identity);
        }
    }

}
