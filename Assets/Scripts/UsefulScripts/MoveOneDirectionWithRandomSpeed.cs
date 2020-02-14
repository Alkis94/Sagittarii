using UnityEngine;
using System.Collections;

public class MoveOneDirectionWithRandomSpeed : MonoBehaviour
{

    [SerializeField]
    private float MoveX = 1;
    [SerializeField]
    private float MoveY = 1;

    // Use this for initialization
    void Start()
    {
        MoveX = Random.Range(-0.75f, -1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(MoveX * Time.deltaTime, MoveY * Time.deltaTime, 0);
    }
}
