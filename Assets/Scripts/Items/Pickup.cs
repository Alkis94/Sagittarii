using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public AudioSource PickedupSound;
    protected Rigidbody2D PickupRB2D;
    protected SpriteRenderer PickupSpriteRender;
    private Object TemporaryParticleHandler;
    private float DestroyTemporaryParticleHandlerTimer;
    private int DespawnDelay;
    private bool PlayerNotPickedUp;

    void Start()
    {
        PlayerNotPickedUp = true;
        DespawnDelay = C.PICKUP_DESPAWN_DELAY;
        PickupRB2D = GetComponent<Rigidbody2D>();
        PickupSpriteRender = GetComponent<SpriteRenderer>();
        Destroy(gameObject, DespawnDelay);
    }

    void Update()
    {
        DestroyTemporaryParticleHandlerTimer += Time.deltaTime;
    }

    public void Initialize(Transform deadEnemyPosition)
    {
        transform.position = deadEnemyPosition.transform.position;
    }

    protected void PickupPlayerCollision()
    {
        PickedupSound.Play();
        PickupSpriteRender.enabled = false;
        Destroy(gameObject, 0.3f);
        Destroy(TemporaryParticleHandler);
    }

    protected void PickupGroundCollision()
    {
        PickupRB2D.velocity = Vector2.zero;
        PickupRB2D.angularVelocity = 0;
        PickupRB2D.isKinematic = true;
        TemporaryParticleHandler = ObjectFactory.Instance.CreatePickupParticles(transform);
        DestroyTemporaryParticleHandlerTimer = DespawnDelay - DestroyTemporaryParticleHandlerTimer;
        Destroy(TemporaryParticleHandler, DestroyTemporaryParticleHandlerTimer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerNotPickedUp = false;
            PickupPlayerCollision();
        }
        else if (other.tag == "Ground" && PlayerNotPickedUp)
        {
            PickupGroundCollision();
        }
    }


}
