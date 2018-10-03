using UnityEngine;
using System.Collections;

public class Arrow : Projectile
{
    private Collider2D ArrowCollider2D;
    public AudioSource ArrowImpact;
    public AudioSource ArrowGroundImpact;

    private float ArrowAngle;
    private float ArrowSpeed;
    private float ArrowPower;




    void Start ()
    {
        transform.GetChild(0).gameObject.SetActive(ItemHandler.Instance.PlayerHasImpFlame);
        ProjectileRB2D = GetComponent<Rigidbody2D>();
        ArrowCollider2D = GetComponent<Collider2D>();
        ArrowSpeed = C.ARROW_PROJECTILE_SPEED;
        ProjectileRB2D.AddForce(transform.right * ArrowSpeed * ArrowPower);
        ProjectileRB2D.AddForce(transform.up * ArrowSpeed * ArrowPower * VerticalFactor);
        Destroy(gameObject, 20.0f);
    }
	


    void Update ()
    {
        Vector2 v = ProjectileRB2D.velocity;
        ArrowAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(ArrowAngle, Vector3.forward);
    }

    public void Initialize(float arrowPower,GameObject arrowEmitter,float verticalFactor)
    {
        ArrowPower = arrowPower;
        transform.position = arrowEmitter.transform.position;
        transform.rotation = arrowEmitter.transform.rotation;
        VerticalFactor = verticalFactor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ArrowCollider2D.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        ProjectileImpact(10f);
        if (other.tag == "Enemy")
        {
            transform.parent = other.transform;
            ArrowImpact.Play();
        }
        else
        {
            ArrowGroundImpact.Play();
        }
    }
}
