using UnityEngine;
using UnityEditor;

public class ApplicationManager : MonoBehaviour
{

    private void Start()
    {
        EditorApplication.playModeStateChanged += ModeChanged;
    }

    private void OnApplicationQuit()
    {
        ES3.DeleteDirectory("Levels/");
        ES3.DeleteDirectory("Bosses/");
    }

    private void ModeChanged(PlayModeStateChange playModeState)
    {
        if (playModeState == PlayModeStateChange.ExitingEditMode)
        {
            ES3.DeleteDirectory("Levels/");
            ES3.DeleteDirectory("Bosses/");
        }
    }
}
