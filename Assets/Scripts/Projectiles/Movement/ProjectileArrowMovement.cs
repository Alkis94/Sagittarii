using UnityEngine;

public class ProjectileArrowMovement : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    private float ArrowAngle;

 

    private Projectile projectile;

    private void OnEnable()
    {
        GetComponent<ProjectilePlayerTrigger>().OnCollision += DisableComponent;
    }

    void Start()
    {
        projectile = GetComponent<Projectile>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.AddForce(transform.right * projectile.Speed);
    }


    void Update()
    {
        Vector2 v = rigidbody2d.velocity;
        ArrowAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(ArrowAngle, Vector3.forward);
    }

    private void DisableComponent()
    {
        enabled = false;
    }

}
