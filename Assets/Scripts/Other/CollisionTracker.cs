using UnityEngine;

public class CollisionTracker : MonoBehaviour
{
    public LayerMask collisionMask;
    public CollisionInfo collisions;
    private Raycaster raycaster;
    [SerializeField]
    private float horizontalRayLength = 0.1f;

    void Awake()
    {
        raycaster = GetComponent<Raycaster>();
    }

    public void TrackHorizontalCollisions()
    {
        var horizontalDirection = transform.parent.localRotation.y == 0 ? 1 : -1;
        var directionX = (int)Mathf.Sign(horizontalDirection);
        
        for (var i = 0; i < raycaster.horizontalRayCount; i++)
        {
            var rayOrigin = (directionX == -1) ? raycaster.raycastOrigins.bottomLeft : raycaster.raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (raycaster.horizontalRaySpacing * i);
            var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, horizontalRayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    public void TrackVerticalCollisions(float velocityY)
    {
        var directionY = velocityY > 0 ? 1 : -1;
        var rayLength = 3*Raycaster.skinWidth;

        for (var i = 0; i < raycaster.verticalRayCount; i++)
        {
            var rayOrigin = (directionY == -1) ? raycaster.raycastOrigins.bottomLeft  : raycaster.raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (raycaster.verticalRaySpacing * i);
            var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
            }
        }
    }

    public bool CloseToGroundEdge()
    {
        var rayLength = 2 * Raycaster.skinWidth;
        var horizontalDirection = transform.parent.localRotation.y == 0 ? 1 : -1;

        var rayOrigin = raycaster.raycastOrigins.bottomLeft;
        var FirstRayHit = Physics2D.Raycast(rayOrigin, -Vector2.up, rayLength, collisionMask);
        Debug.DrawRay(rayOrigin, -Vector2.up, Color.red);


        rayOrigin = raycaster.raycastOrigins.bottomRight;
        var LastRayHit = Physics2D.Raycast(rayOrigin, -Vector2.up, rayLength, collisionMask);
        Debug.DrawRay(rayOrigin, -Vector2.up, Color.red);

        if (FirstRayHit.distance > LastRayHit.distance && horizontalDirection > 0)
        {
            return true;
        }
        else if (FirstRayHit.distance < LastRayHit.distance && horizontalDirection < 0)
        {
            return true;
        }

        return false;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public int facingDirection;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
