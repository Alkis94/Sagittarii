using UnityEngine;

public class ProjectileArrowMovement : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    private float arrowAngle;

 

    private Projectile projectile;

    private void OnEnable()
    {
        GetComponent<PlayerProjectileImpact>().OnCollision += DisableComponent;
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
        arrowAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(arrowAngle, Vector3.forward);
    }

    private void DisableComponent()
    {
        enabled = false;
    }

}
