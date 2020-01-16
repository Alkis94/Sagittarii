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

    public static Transform DestroyAllChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
}
