using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player;       //Public variable to store a reference to the player game object


    private float offset;         //Private variable to store the offset distance between the player and camera
    private Vector3 NewCameraPos;

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position.x - player.transform.position.x;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        NewCameraPos = new Vector3(player.transform.position.x + offset, transform.position.y,-10);
        transform.position = NewCameraPos;
    }
}
