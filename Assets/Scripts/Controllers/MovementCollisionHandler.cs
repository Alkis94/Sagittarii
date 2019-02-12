using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementCollisionHandler : MonoBehaviour
{

    public LayerMask collisionMask;
    public CollisionInfo collisions;
    private Raycaster raycaster;

    private bool fallingThroughPlatform;


    void Start()
    {
        raycaster = GetComponent<Raycaster>();
        collisions.facingDirection = 1;
    }

    public void HandleHorizontalCollisions(ref float moveAmountX)
    {
        float directionX = collisions.facingDirection;
        float rayLength = Mathf.Abs(moveAmountX) + Raycaster.skinWidth;

        if (Mathf.Abs(moveAmountX) < Raycaster.skinWidth)
        {
            rayLength = 2 * Raycaster.skinWidth;
        }

        for (int i = 0; i < raycaster.horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycaster.raycastOrigins.bottomLeft : raycaster.raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (raycaster.horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                if (hit.distance != 0)
                {
                    moveAmountX = (hit.distance - Raycaster.skinWidth) * directionX;
                    rayLength = hit.distance;
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }



    public void HandleVerticalCollisions(ref float moveAmountY,bool GoThroughPlatform)
    {
        float directionY = Mathf.Sign(moveAmountY);
        float rayLength = Mathf.Abs(moveAmountY) + Raycaster.skinWidth;

        for (int i = 0; i < raycaster.verticalRayCount; i++)
        {

            Vector2 rayOrigin = (directionY == -1) ? raycaster.raycastOrigins.bottomLeft : raycaster.raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (raycaster.verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                if(!PassingThroughPlatform(hit, GoThroughPlatform, directionY))
                {
                    moveAmountY = (hit.distance - Raycaster.skinWidth) * directionY;
                    rayLength = hit.distance;

                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
                }
                
            }
        }
    }

    public bool CloseToGroundEdge()
    {
        float directionY = -1;
        float rayLength = 3 * Raycaster.skinWidth;

        Vector2 rayOrigin = raycaster.raycastOrigins.bottomLeft;
        RaycastHit2D FirstRayHit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
        //Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

        rayOrigin += Vector2.right * (raycaster.verticalRaySpacing * (raycaster.verticalRayCount - 1));
        RaycastHit2D LastRayHit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
        //Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

        if (FirstRayHit.distance > LastRayHit.distance)
        {
            return true;
        }
        else if (FirstRayHit.distance < LastRayHit.distance)
        {
            return true;
        }

        return false;
    }

    private bool PassingThroughPlatform(RaycastHit2D hit, bool GoThroughPlatform, float directionY)
    {

        if (hit.collider.tag == "PassablePlatform")
        {
            if (directionY == 1 || hit.distance == 0)
            {
                return true;
            }
            if (fallingThroughPlatform)
            {
                return true;
            }
            if (GoThroughPlatform)
            {
                fallingThroughPlatform = true;
                StartCoroutine(ResetFallingThroughPlatform());
                return true;
            }
        }
        return false;
    }

    IEnumerator ResetFallingThroughPlatform()
    {
        yield return new WaitForSeconds(0.5f);
        fallingThroughPlatform = false;
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
