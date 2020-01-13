using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GetCameraConfiner : MonoBehaviour
{
    private PolygonCollider2D cameraConfiner;
    private CinemachineConfiner cinemachineConfiner;

    private void Awake()
    {
        cinemachineConfiner = GetComponent<CinemachineConfiner>();
    }

    private void Start()
    {
        
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
        GameObject tempConfiner = GameObject.FindGameObjectWithTag("CameraConfiner");
        if(tempConfiner != null)
        {
            cameraConfiner = tempConfiner.GetComponent<PolygonCollider2D>();
            cinemachineConfiner.m_BoundingShape2D = cameraConfiner;
            cinemachineConfiner.InvalidatePathCache();
        }
    }

}
