using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Shaker(3f, 3f));
    }



    public static IEnumerator Shake (float duration, float magnitude)
    {
        Debug.Log("Should shake camera!");
        Transform camera = FindObjectOfType<CameraShake>().transform;

        Vector3 originalPos = camera.position;

        float timeElapsed = 0.0f;

        while(timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            camera.position = new Vector3(x, y, originalPos.z);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        camera.position = originalPos;
    }

    public IEnumerator Shaker(float duration, float magnitude)
    {

        Debug.Log("Called shaker");
        Vector3 originalPos = transform.localPosition;

        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
