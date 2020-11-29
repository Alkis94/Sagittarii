using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip townMusic;
    [SerializeField]
    private AudioClip forestMusic;
    [SerializeField]
    private AudioClip bearBossMusic;
    [SerializeField]
    private AudioClip caveMusic;

    private MapType mapType;
    private RoomType roomType;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayTheRightMusic(SceneManager.GetActiveScene());
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Town")
        {
            mapType = MapManager.Instance.CurrentMap;
            roomType = MapManager.Instance.GetMapRoomType();
        }
        PlayTheRightMusic(scene);
    }

    private void PlayTheRightMusic(Scene scene)
    {
        if (scene.name == "Town")
        {
            if (audioSource.clip != townMusic)
            {
                audioSource.FadeOut(2);
                StartCoroutine(PlayMusicWithDelay(townMusic, 3));
            }
        }
        else if (scene.name == "BearBoss")
        {
            if (audioSource.clip != bearBossMusic)
            {
                audioSource.FadeOut(2);
                StartCoroutine(PlayMusicWithDelay(bearBossMusic, 3));
            }
        }
        else if (mapType == MapType.forest)
        {
            if (audioSource.clip != forestMusic)
            {
                audioSource.FadeOut(2);
                StartCoroutine(PlayMusicWithDelay(forestMusic, 3));
            }
        }
        else if (mapType == MapType.cave)
        {
            if (audioSource.clip != caveMusic)
            {
                audioSource.FadeOut(2);
                StartCoroutine(PlayMusicWithDelay(caveMusic, 3));
            }
        }
    }

    IEnumerator PlayMusicWithDelay(AudioClip audioClip,float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}