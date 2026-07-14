using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance = null;

    private int SaveId  = 1;
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

    public void SetSaveId(int saveId)
    {
        SaveId = saveId;
    }

    public string GetProfileRunPath()
    {
        return SaveFolders.SaveProfile + SaveId + "/Run/";
    }

    public string GetProfileUnlocksPath()
    {
        return SaveFolders.SaveProfile + SaveId + "/Unlocks/";
    }
}
