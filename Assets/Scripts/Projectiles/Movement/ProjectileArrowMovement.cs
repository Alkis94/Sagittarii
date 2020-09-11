using UnityEngine;

public class ProjectileArrowMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    private void OnEnable()
    {
        GetComponent<ProjectileHandler>().OnCollision += DisableComponent;
    }

    private void OnDisable()
    {
        GetComponent<ProjectileHandler>().OnCollision -= DisableComponent;
    }

    void Start()
    {
        float speed = GetComponent<ProjectileDataInitializer>().Speed;
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.gravityScale = 1;
        rigidbody2d.AddForce(transform.right * speed * 100);
    }

    void Update()
    {
        Vector2 velocity = rigidbody2d.velocity;
        float arrowAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(arrowAngle, Vector3.forward);
    }

    private void DisableComponent()
    {
        enabled = false;
    }

}
