using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GetComponent<MapCreator>().enabled = true;
        }
    }


}
