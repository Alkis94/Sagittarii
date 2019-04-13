using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomFinish : MonoBehaviour
{
    public static event Action OnRoomFinished = delegate { };
    private Spawner spawner;
    private AudioSource audioSource;


    private void Awake()
    {
        GameObject spawnerObject = GameObject.FindGameObjectWithTag("Spawner");
        if(spawnerObject != null)
        {
            spawner = spawnerObject.GetComponent<Spawner>();
        }   
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(spawner != null)
        {
            spawner.OnSpawnerFinished += StartCheckingEnemies;
        }
        else
        {
            StartCoroutine(CheckEnemiesLeft());
        }
    }

    private void OnDisable()
    {
        if (spawner != null)
        {
            spawner.OnSpawnerFinished -= StartCheckingEnemies;
        }
    }

    private void StartCheckingEnemies()
    {
       StartCoroutine(CheckEnemiesLeft());
    }

    IEnumerator CheckEnemiesLeft ()
    {
        while(true)
        {
            if(transform.childCount <= 0)
            {
                //Debug.Log("Room Finished");
                StartCoroutine(PlayFinishSound());
                OnRoomFinished?.Invoke();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator PlayFinishSound()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.Play();
    }

}
