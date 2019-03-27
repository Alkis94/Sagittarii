using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class RoomPressed : MonoBehaviour
{
    public static event Action<Vector3> OnRoomChosen;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnUnExploredRoomPressed()
    {
        button.interactable = false;
        int index = UnityEngine.Random.Range(2, 7);
        SceneManager.LoadScene(index);
        OnRoomChosen?.Invoke(transform.localPosition);
    }
}
