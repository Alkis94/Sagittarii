using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance { get; private set; } = null;
    private RectTransform rectTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void LoadSceneWithFade(string scene)
    {
        StartCoroutine(StartLoadSceneWithFade(scene));
    }

    IEnumerator StartLoadSceneWithFade(string scene)
    {
        LeanTween.alpha(rectTransform, 1f, 0.5f).setEase(LeanTweenType.linear);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
        LeanTween.alpha(rectTransform, 0f, 0.5f).setEase(LeanTweenType.linear);
    }
}
