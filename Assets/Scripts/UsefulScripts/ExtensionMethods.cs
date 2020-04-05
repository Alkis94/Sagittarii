using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    public static GameObject InstantiateAtLocalPosition(GameObject gameObject, Transform parent, Vector2 localPosition)
    {
        GameObject newObject = GameObject.Instantiate(gameObject, parent) as GameObject;
        newObject.transform.localPosition = localPosition;
        return newObject;
    }

    public static void DestroyAllChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void Impulse(this Rigidbody2D rigidbody2d, float horizontalForce, float verticalForce)
    {
        rigidbody2d.AddForce(new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse);
    }

}
