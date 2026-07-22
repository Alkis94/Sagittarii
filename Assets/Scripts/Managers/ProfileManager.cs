using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance = null;

    public int ProfileId { get; private set; } = 1;
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

        if (ES3.FileExists("CurrentProfile"))
        {
            ProfileId = ES3.Load<int>("ProfileId", "CurrentProfile");
        }
    }

    public void SetProfileId(int id)
    {
        ProfileId = id;
        ES3.Save("ProfileId", ProfileId, "CurrentProfile");
    }

    public string GetProfileRunPath()
    {
        return SaveFolders.Profile + ProfileId + "/Run";
    }

    public bool ProfileHasActiveRun()
    {
        return ES3.DirectoryExists(GetProfileRunPath());
    }

    public string GetProfileUnlocksPath()
    {
        return SaveFolders.Profile + ProfileId + "/Unlocks";
    }

    public void DeleteRun()
    {
        if (ES3.DirectoryExists(SaveFolders.Profile + ProfileId + "/Run"))
        {
            ES3.DeleteDirectory(SaveFolders.Profile + ProfileId + "/Run");
        }
    }

    public void DeleteProfile(int id)
    {
        if (ES3.DirectoryExists(SaveFolders.Profile + id))
        {
            ES3.DeleteDirectory(SaveFolders.Profile + id);
        }
    }
}
