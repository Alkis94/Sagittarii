using UnityEngine;
using System.Collections;

public class ProjectileSpiralMovement : MonoBehaviour
{
    [SerializeField]
    private float curveIntensity = 10f; // (0.01 - 0.99 slower curves, 1+ faster curves)
    private float curveLength = 0;
    private float steps = 0;
    private float speed;
    private Rigidbody2D rigidbody2d;
    private Transform childTransform;

    private void OnEnable()
    {
        GetComponent<ProjectileHandler>().OnCollision += DisableComponent;
    }

    private void OnDisable()
    {
        GetComponent<ProjectileHandler>().OnCollision -= DisableComponent;
    }

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        speed = GetComponent<ProjectileDataInitializer>().Speed;
        rigidbody2d.velocity = new Vector2(speed * transform.right.x, speed * transform.right.y);
    }


    private void FixedUpdate()
    {
        steps += 20;
        if (steps > curveLength)
        {
            steps = 0;
            transform.localRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + curveIntensity);
            rigidbody2d.velocity = new Vector2(speed * transform.right.x, speed * transform.right.y);
            curveLength++;
        }
    }

    private void DisableComponent()
    {
        enabled = false;
    }
}
