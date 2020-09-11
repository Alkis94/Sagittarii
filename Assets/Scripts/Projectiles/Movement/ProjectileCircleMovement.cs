using UnityEngine;

public class ProjectileCircleMovement : MonoBehaviour
{
    [SerializeField]
    private float curveIntensity = 2f; // (0.01 - 0.99 slower curves, 1+ faster curves)
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

        if (Random.Range(0f, 1f) < 0.5f)
        {
            curveIntensity *= -1;
        }

    }


    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + curveIntensity);
        rigidbody2d.velocity = new Vector2(speed * transform.right.x, speed * transform.right.y);
    }

    private void DisableComponent()
    {
        enabled = false;
    }
}

