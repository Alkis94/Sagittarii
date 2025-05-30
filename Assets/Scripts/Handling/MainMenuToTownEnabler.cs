﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuToTownEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject playerUI;
    [SerializeField]
    private GameObject character;
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private GameObject itemFactories;
    [SerializeField]
    private GameObject musicManager;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            playerUI.SetActive(true);
            playerCamera.SetActive(true);
            character.transform.GetChild(0).gameObject.SetActive(true);
            itemFactories.SetActive(true);
            musicManager.SetActive(true);
            Destroy(gameObject);
        }
    }
}
