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

        var characterChooser = FindObjectOfType<CharacterChooser>();
        if (characterChooser != null)
        {
            GetComponent<CinemachineVirtualCamera>().Follow = characterChooser.transform.GetChild(0);
        }
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
        var confiner = GameObject.FindGameObjectWithTag("CameraConfiner");
        if (confiner != null)
        {
            cameraConfiner = confiner.GetComponent<PolygonCollider2D>();
            cinemachineConfiner.m_BoundingShape2D = cameraConfiner;
            cinemachineConfiner.InvalidatePathCache();
        }
    }
}
