using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnexploredRoomPressed : MonoBehaviour
{
    public void OnUnExploredRoomPressed()
    {
        int index = Random.Range(2, 7);
        SceneManager.LoadScene(index);
    }
}
