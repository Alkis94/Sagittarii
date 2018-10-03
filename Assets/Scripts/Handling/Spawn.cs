using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{
    

    public int DifficultyIncreaseDelay;


    public IEnumerator SpawnFlyingEnemy(GameObject enemyPrefab,int frequency,int delay)
    {
        yield return new WaitForSeconds(delay);
        float TimeUntilFrequencyIncrease = 0;
        while (true)
        {
            
            Vector3 screenPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), Random.Range( 0.4f, 0.80f), 10f));
            ObjectFactory.Instance.CreateFlyingEnemy(enemyPrefab, screenPosition);
            yield return new WaitForSeconds(frequency);

            TimeUntilFrequencyIncrease += frequency;
            if (TimeUntilFrequencyIncrease > DifficultyIncreaseDelay)
            {
                frequency = frequency <= 1 ? 1 : frequency-1;
                TimeUntilFrequencyIncrease = 0;
            }
        }
    }

    public IEnumerator SpawnGroundEnemy<T>(int frequency, int delay,Transform SpawnPoint1, Transform SpawnPoint2,GameObject enemy) where T : IInitializable
    {
        yield return new WaitForSeconds(delay);
        int RandomNumber;
        float TimeUntilFrequencyIncrease = 0;
        while (true)
        {
            RandomNumber = Random.Range(1, 3);
            if (RandomNumber == 1)
            {
                ObjectFactory.Instance.CreateGroundEnemy<T>(SpawnPoint1, 1, enemy);
            }
            else
            {
                ObjectFactory.Instance.CreateGroundEnemy<T>(SpawnPoint2, -1, enemy);
            }
            yield return new WaitForSeconds(frequency);

            TimeUntilFrequencyIncrease += frequency;
            if (TimeUntilFrequencyIncrease > DifficultyIncreaseDelay)
            {
                frequency = frequency <= 1 ? 1 : frequency - 1;
                TimeUntilFrequencyIncrease = 0;
            }
        }
    }
}







