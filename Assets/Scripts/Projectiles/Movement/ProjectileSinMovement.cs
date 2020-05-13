using UnityEngine;
using System.Collections;

public class ProjectileSinMovement : MonoBehaviour
{

    [SerializeField]
    private float curveIntensity = 5f; // (0.01 - 0.99 slower curves, 1+ faster curves)
    private float curveLength = 20;
    private float originalCurve = 0;
    private float steps = 0;
    private float speed;
    private Rigidbody2D rigidbody2d;
    private Transform childTransform;

    private void OnEnable()
    {
        GetComponent<ProjectileImpact>().OnCollision += DisableComponent;
    }

    private void OnDisable()
    {
        GetComponent<ProjectileImpact>().OnCollision -= DisableComponent;
    }

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        speed = GetComponent<ProjectileDataInitializer>().Speed;
        rigidbody2d.velocity = new Vector2(speed * transform.right.x, speed * transform.right.y);
        originalCurve = transform.localEulerAngles.z;
        steps = curveLength / 2;

        if(Random.Range(0f, 1f) < 0.5f)
        {
            curveIntensity *= -1;
        }

    }


    private void FixedUpdate()
    {

        transform.localRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + curveIntensity);
        rigidbody2d.velocity = new Vector2(speed * transform.right.x, speed * transform.right.y);

        steps++;
        if (steps > curveLength)
        {
            steps = 0;
            curveIntensity *= -1;
        }

    }

    private void DisableComponent()
    {
        enabled = false;
    }
}
