using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private PolygonCollider2D cameraConfiner;
    private CinemachineConfiner cinemachineConfiner;

    private void Awake()
    {
        cinemachineConfiner = GetComponent<CinemachineConfiner>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GetConfiner();
        GetComponent<CinemachineVirtualCamera>().Follow = FindObjectOfType<CharacterChooser>().transform.GetChild(0);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetConfiner();
    }

    private void GetConfiner()
    {
        GameObject tempConfiner = GameObject.FindGameObjectWithTag("CameraConfiner");
        if (tempConfiner != null)
        {
            cameraConfiner = tempConfiner.GetComponent<PolygonCollider2D>();
            cinemachineConfiner.m_BoundingShape2D = cameraConfiner;
            cinemachineConfiner.InvalidatePathCache();
        }
    }

}
