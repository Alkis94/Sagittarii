using UnityEngine;
using System.Collections;


public class Raycaster : MonoBehaviour
{

    public const float skinWidth = .03f;
    const float distanceBetweenRays = .5f;

	[HideInInspector]
	public int horizontalRayCount;
	[HideInInspector]
	public int verticalRayCount;

	[HideInInspector]
	public float horizontalRaySpacing;
	[HideInInspector]
	public float verticalRaySpacing;

	[HideInInspector]
	public RaycastOrigins raycastOrigins;

    private void Start()
    {
        CalculateRaySpacing();
        UpdateRaycastOrigins();
    }

    public void UpdateRaycastOrigins()
    {
		Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}
	
	public void CalculateRaySpacing()
    {
		Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;
		
		horizontalRayCount = Mathf.RoundToInt (boundsHeight / distanceBetweenRays);
		verticalRayCount = Mathf.RoundToInt (boundsWidth / distanceBetweenRays);

        if(horizontalRayCount < 2)
        {
            horizontalRayCount = 2;
        }

        if(verticalRayCount < 2)
        {
            verticalRayCount = 2;
        }
		
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
	
	public struct RaycastOrigins
    {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
