using UnityEngine;

public class DestroyMenu : MonoBehaviour
{

    private void OnEnable()
    {
        GetComponentInChildren<Resume>().OnResumePressed += DestroyThisMenu;
    }

    private void OnDisable()
    {
        GetComponentInChildren<Resume>().OnResumePressed -= DestroyThisMenu;
    }


    private void DestroyThisMenu()
    {
        Destroy(gameObject);
    }

}
