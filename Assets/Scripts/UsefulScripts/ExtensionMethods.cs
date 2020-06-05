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

    public static void SetBoolArrayToTrue(ref bool[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = true;
            }
        }
    }

    public static void Impulse(this Rigidbody2D rigidbody2d, float horizontalForce, float verticalForce)
    {
        rigidbody2d.AddForce(new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse);
    }

    public static bool StringEndsWith(this string a, string b)
    {
        int aCounter = a.Length - 1;
        int bCounter = b.Length - 1;

        while (aCounter >= 0 && bCounter >= 0 && a[aCounter] == b[bCounter])
        {
            aCounter--;
            bCounter--;
        }

        return (bCounter < 0);
    }

    public static bool StringStartsWith(this string a, string b)
    {
        int aLength = a.Length;
        int bLength = b.Length;

        int aCounter = 0;
        int bCounter = 0;

        while (aCounter < aLength && bCounter < bLength && a[aCounter] == b[bCounter])
        {
            aCounter++;
            bCounter++;
        }

        return (bCounter == bLength);
    }

}
