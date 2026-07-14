using UnityEngine;

public class MoveOneDirectionWithRandomSpeed : MonoBehaviour
{

    [SerializeField]
    private float MoveX = 1;
    [SerializeField]
    private float MoveY = 1;

    void Start()
    {
        MoveX = Random.Range(-0.75f, -1.5f);
    }

    void Update()
    {
        transform.position += new Vector3(MoveX * Time.deltaTime, MoveY * Time.deltaTime, 0);
    }
}
