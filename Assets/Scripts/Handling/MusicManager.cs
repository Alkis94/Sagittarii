using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip forestMusic;
    [SerializeField]
    private AudioClip townMusic;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().name == "Town" )
        {
            if (audioSource.clip != townMusic)
            {
                StartCoroutine(PlayMusicWithDelay(townMusic, 5));
            }
        }
        else if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            audioSource.Stop();
        }
        else
        {
            if(audioSource.clip != forestMusic)
            {
                StartCoroutine(PlayMusicWithDelay(forestMusic, 3));
            }
        }
    }


    IEnumerator PlayMusicWithDelay(AudioClip audioClip,float delay)
    {
        audioSource.Stop();
        yield return new WaitForSeconds(delay);
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}