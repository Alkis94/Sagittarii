using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        ES3.DeleteDirectory("Levels/");
    }
}
