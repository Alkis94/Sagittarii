using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTracker : MonoBehaviour
{

    public LayerMask collisionMask;
    public CollisionInfo collisions;
    private Raycaster raycaster;

    void Start()
    {
        raycaster = GetComponent<Raycaster>();
    }

    public void TrackHorizontalCollisions()
    {
        int horizontalDirection = transform.parent.localRotation.y == 0 ? 1 : -1;

        int directionX = (int) Mathf.Sign(horizontalDirection);
        float rayLength =  3*Raycaster.skinWidth;

        for (int i = 0; i < raycaster.horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycaster.raycastOrigins.bottomLeft : raycaster.raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (raycaster.horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

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

        int directionY = velocityY > 0 ? 1 : -1;
        float rayLength = 3*Raycaster.skinWidth;

        for (int i = 0; i < raycaster.verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycaster.raycastOrigins.bottomLeft  : raycaster.raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (raycaster.verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

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
        float rayLength = 2 * Raycaster.skinWidth;

        int horizontalDirection = transform.parent.localRotation.y == 0 ? 1 : -1;

        Vector2 rayOrigin = raycaster.raycastOrigins.bottomLeft;
        RaycastHit2D FirstRayHit = Physics2D.Raycast(rayOrigin, -Vector2.up, rayLength, collisionMask);
        //Debug.DrawRay(rayOrigin, -Vector2.up, Color.red);


        rayOrigin = raycaster.raycastOrigins.bottomRight;
        RaycastHit2D LastRayHit = Physics2D.Raycast(rayOrigin, -Vector2.up, rayLength, collisionMask);
        //Debug.DrawRay(rayOrigin, -Vector2.up, Color.red);

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
